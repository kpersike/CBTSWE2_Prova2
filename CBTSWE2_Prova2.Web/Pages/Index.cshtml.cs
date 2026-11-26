/*
Nome: Kaik Persike Maiorquino
Prontuário: CB3029689

Nome: Luiz Gustavo Verissimo Monteiro
Prontuário: CB3030326
*/
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CBTSWE2_Prova2.Core.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTSWE2_Prova2.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<ProdutoResponseDTO> Produtos
        {
            get; set;
        } = new();

        public ProdutoCadastroDTO NovoProduto
        {
            get; set;
        } = new();

        public ProdutoUpdateDTO ProdutoEdicao
        {
            get; set;
        } = new();

        public int IdProdutoEdicao { get; set; }

        public bool AbrirModalEdicao { get; set; }

        public bool AbrirModalCadastro { get; set; }

        [TempData]
        public string? MensagemSucesso { get; set; }

        [TempData]
        public string? MensagemErro { get; set; }

        private HttpClient? ObterHttpClient()
        {
            var token = HttpContext.Session
                .GetString("Token");

            if (string.IsNullOrWhiteSpace(token))
                return null;

            var httpClient = _httpClientFactory
                .CreateClient("ProdutosAPI");

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer", token
                );

            return httpClient;
        }

        private async Task<IActionResult>
            EncerrarSessaoAsync()
        {
            HttpContext.Session.Clear();

            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults
                    .AuthenticationScheme
            );

            return RedirectToPage("/Login");
        }

        private async Task<bool> CarregarProdutosAsync(
            HttpClient httpClient)
        {
            using var response =
                await httpClient.GetAsync("api/Produtos");

            if (response.StatusCode ==
                HttpStatusCode.Unauthorized ||
                response.StatusCode ==
                HttpStatusCode.Forbidden)
            {
                return false;
            }

            if (!response.IsSuccessStatusCode)
            {
                MensagemErro =
                    "Não foi possível consultar os produtos.";

                return true;
            }

            Produtos = await response.Content
                .ReadFromJsonAsync<List<ProdutoResponseDTO>>()
                ?? new List<ProdutoResponseDTO>();

            return true;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            using var httpClient = ObterHttpClient();

            if (httpClient == null)
                return await EncerrarSessaoAsync();

            try
            {
                if (!await CarregarProdutosAsync(httpClient))
                    return await EncerrarSessaoAsync();
            }
            catch (Exception)
            {
                MensagemErro =
                    "Não foi possível comunicar com a API.";
            }

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(
            [Bind(Prefix = "NovoProduto")]
            ProdutoCadastroDTO novoProduto)
        {
            NovoProduto = novoProduto;

            using var httpClient = ObterHttpClient();

            if (httpClient == null)
                return await EncerrarSessaoAsync();

            try
            {
                if (!ModelState.IsValid)
                {
                    var erros = ModelState
                        .Where(x => x.Value != null &&
                            x.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value!.Errors.Select(
                            erro => $"{x.Key}: {erro.ErrorMessage}"
                        ))
                        .ToList();

                    MensagemErro = string.Join(" | ", erros);

                    Console.WriteLine(
                        $"ERROS DE VALIDAÇÃO: {MensagemErro}"
                    );

                    AbrirModalCadastro = true;

                    if (!await CarregarProdutosAsync(httpClient))
                        return await EncerrarSessaoAsync();

                    return Page();
                }

                Console.WriteLine(
                    $"CADASTRO: Nome={NovoProduto.Nome}, " +
                    $"Preco={NovoProduto.Preco}, " +
                    $"Status={NovoProduto.Status}"
                );

                using var response =
                    await httpClient.PostAsJsonAsync(
                        "api/Produtos", NovoProduto
                    );

                if (response.StatusCode ==
                    HttpStatusCode.Unauthorized ||
                    response.StatusCode ==
                    HttpStatusCode.Forbidden)
                {
                    return await EncerrarSessaoAsync();
                }

                if (!response.IsSuccessStatusCode)
                {
                    var erroApi = await response.Content
                        .ReadAsStringAsync();

                    Console.WriteLine(
                        $"ERRO API: {(int)response.StatusCode} - {erroApi}"
                    );

                    ModelState.AddModelError(
                        string.Empty,
                        $"Não foi possível cadastrar o produto. " +
                        $"Código HTTP: {(int)response.StatusCode}"
                    );

                    AbrirModalCadastro = true;

                    if (!await CarregarProdutosAsync(httpClient))
                        return await EncerrarSessaoAsync();

                    return Page();
                }

                MensagemSucesso =
                    "Produto cadastrado com sucesso!";

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                MensagemErro =
                    "Não foi possível comunicar com a API.";

                return RedirectToPage();
            }
        }


        public async Task<IActionResult> OnPostAtualizarAsync(
            [Bind(Prefix = "ProdutoEdicao")]
            ProdutoUpdateDTO produtoEdicao,
            int idProdutoEdicao)
        {

            ProdutoEdicao = produtoEdicao;
            IdProdutoEdicao = idProdutoEdicao;

            using var httpClient = ObterHttpClient();

            if (httpClient == null)
                return await EncerrarSessaoAsync();

            try
            {
                if (IdProdutoEdicao <= 0)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Produto inválido."
                    );
                }

                if (!ModelState.IsValid)
                {
                    AbrirModalEdicao = true;

                    if (!await CarregarProdutosAsync(httpClient))
                        return await EncerrarSessaoAsync();

                    return Page();
                }

                using var response =
                    await httpClient.PutAsJsonAsync(
                        $"api/Produtos/{IdProdutoEdicao}",
                        ProdutoEdicao
                    );

                if (response.StatusCode ==
                    HttpStatusCode.Unauthorized ||
                    response.StatusCode ==
                    HttpStatusCode.Forbidden)
                {
                    return await EncerrarSessaoAsync();
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    MensagemErro = "Produto não encontrado.";

                    return RedirectToPage();
                }

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Não foi possível atualizar o produto."
                    );

                    AbrirModalEdicao = true;

                    if (!await CarregarProdutosAsync(httpClient))
                        return await EncerrarSessaoAsync();

                    return Page();
                }

                MensagemSucesso =
                    "Produto atualizado com sucesso!";

                return RedirectToPage();
            }
            catch (Exception)
            {
                MensagemErro =
                    "Não foi possível comunicar com a API.";

                return RedirectToPage();
            }
        }


        public async Task<IActionResult> OnPostExcluirAsync(int id)
        {
            using var httpClient = ObterHttpClient();

            if (httpClient == null)
                return await EncerrarSessaoAsync();

            if (id <= 0)
            {
                MensagemErro = "Produto inválido.";

                return RedirectToPage();
            }

            try
            {
                using var response =
                    await httpClient.DeleteAsync(
                        $"api/Produtos/{id}"
                    );

                if (response.StatusCode ==
                    HttpStatusCode.Unauthorized ||
                    response.StatusCode ==
                    HttpStatusCode.Forbidden)
                {
                    return await EncerrarSessaoAsync();
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    MensagemErro = "Produto não encontrado.";

                    return RedirectToPage();
                }

                if (!response.IsSuccessStatusCode)
                {
                    MensagemErro =
                        "Não foi possível excluir o produto.";

                    return RedirectToPage();
                }

                MensagemSucesso =
                    "Produto excluído com sucesso!";

                return RedirectToPage();
            }
            catch (Exception)
            {
                MensagemErro =
                    "Não foi possível comunicar com a API.";

                return RedirectToPage();
            }
        }


    }
}
