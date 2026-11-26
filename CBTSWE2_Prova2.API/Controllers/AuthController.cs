/*
Nome: Kaik Persike Maiorquino
Prontuário: CB3029689

Nome: Luiz Gustavo Verissimo Monteiro
Prontuário: CB3030326
*/
using CBTSWE2_Prova2.API.Data;
using CBTSWE2_Prova2.Core;
using CBTSWE2_Prova2.Core.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CBTSWE2_Prova2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthController(
            AppDbContext context,
            IPasswordHasher<Usuario> passwordHasher,
            IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login(
            LoginDTO dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Nome == dto.Nome);

            if (usuario == null)
                return Unauthorized("Credenciais inválidas.");

            var resultado = _passwordHasher.VerifyHashedPassword(
                usuario,
                usuario.Senha,
                dto.Senha);

            if (resultado == PasswordVerificationResult.Failed)
                return Unauthorized("Credenciais inválidas.");

            if (!usuario.Status)
                return StatusCode(403, "Usuário inativo.");

            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    usuario.Id.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    usuario.Nome)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var minutos = _configuration.GetValue<int>(
                "Jwt:ExpirationMinutes");

            var expiracao = DateTime.UtcNow.AddMinutes(minutos);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiracao,
                signingCredentials: credentials
            );

            return Ok(new LoginResponseDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Token = new JwtSecurityTokenHandler()
                    .WriteToken(token),
                Expiracao = expiracao
            });
        }
    }
}