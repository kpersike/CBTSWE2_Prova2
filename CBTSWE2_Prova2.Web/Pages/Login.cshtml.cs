/*
Nome: Kaik Persike Maiorquino
Prontuário: CB3029689

Nome: Luiz Gustavo Verissimo Monteiro
Prontuário: CB3030326
*/
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using CBTSWE2_Prova2.Core.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTSWE2_Prova2.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public LoginDTO Login { get; set; } = new();

        public IActionResult OnGet()
        {
            if (User.Identity?.IsAuthenticated == true &&
                !string.IsNullOrEmpty(
                    HttpContext.Session.GetString("Token")))
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var httpClient =
                    _httpClientFactory.CreateClient("ProdutosAPI");

                using var response =
                    await httpClient.PostAsJsonAsync(
                        "api/Auth/login", Login
                    );

                if (response.StatusCode ==
                    HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Usuário ou senha inválidos."
                    );

                    return Page();
                }

                if (response.StatusCode ==
                    HttpStatusCode.Forbidden)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Este usuário está inativo."
                    );

                    return Page();
                }

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Não foi possível realizar o login."
                    );

                    return Page();
                }

                var resultado = await response.Content
                    .ReadFromJsonAsync<LoginResponseDTO>();

                if (resultado == null ||
                    string.IsNullOrWhiteSpace(resultado.Token))
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Resposta inválida da API."
                    );

                    return Page();
                }

                // Identificação do usuário autenticado.
                var claims = new List<Claim>
                {
                    new Claim(
                        ClaimTypes.NameIdentifier,
                        resultado.Id.ToString()
                    ),

                    new Claim(
                        ClaimTypes.Name,
                        resultado.Nome
                    )
                };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults
                        .AuthenticationScheme
                );

                var principal = new ClaimsPrincipal(identity);

                var propriedades = new AuthenticationProperties
                {
                    IsPersistent = false,
                    AllowRefresh = false,
                    ExpiresUtc = new DateTimeOffset(
                        resultado.Expiracao.ToUniversalTime()
                    )
                };

                // Armazena o JWT na sessão do servidor.
                HttpContext.Session.Clear();

                HttpContext.Session.SetString(
                    "Token", resultado.Token
                );

                // Cria o cookie de autenticação.
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults
                        .AuthenticationScheme,
                    principal,
                    propriedades
                );

                return RedirectToPage("/Index");
            }
            catch (Exception)
            {
                HttpContext.Session.Clear();

                ModelState.AddModelError(
                    string.Empty,
                    "Não foi possível comunicar com a API."
                );

                return Page();
            }
        }
    }
}
