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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CBTSWE2_Prova2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "SomenteAdministrador")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public UsuariosController(
            AppDbContext context,
            IPasswordHasher<Usuario> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // GET: api/usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDTO>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Select(u => new UsuarioResponseDTO
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Status = u.Status
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        // GET: api/usuarios/1
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponseDTO>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            return Ok(new UsuarioResponseDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Status = usuario.Status
            });
        }

        // POST: api/usuarios
        [HttpPost]
        public async Task<ActionResult<UsuarioResponseDTO>> PostUsuario(
            UsuarioCadastroDTO dto)
        {
            bool nomeExiste = await _context.Usuarios
                .AnyAsync(u => u.Nome == dto.Nome);

            if (nomeExiste)
                return Conflict("Já existe um usuário com esse nome.");

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Status = dto.Status
            };

            usuario.Senha = _passwordHasher.HashPassword(
                usuario, dto.Senha);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var response = new UsuarioResponseDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Status = usuario.Status
            };

            return CreatedAtAction(
                nameof(GetUsuario),
                new { id = usuario.Id },
                response);
        }

        // PUT: api/usuarios/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(
            int id, UsuarioUpdateDTO dto)
        {

            var idUsuarioLogado = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            if (id == idUsuarioLogado && !dto.Status)
            {
                return BadRequest(new
                {
                    mensagem = "O administrador não pode inativar a própria conta."
                });
            }

            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            bool nomeExiste = await _context.Usuarios
                .AnyAsync(u => u.Nome == dto.Nome && u.Id != id);

            if (nomeExiste)
                return Conflict("Já existe um usuário com esse nome.");

            usuario.Nome = dto.Nome;
            usuario.Status = dto.Status;

            if (!string.IsNullOrWhiteSpace(dto.Senha))
            {
                usuario.Senha = _passwordHasher.HashPassword(
                    usuario, dto.Senha);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/usuarios/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {

            var idUsuarioLogado = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            if (id == idUsuarioLogado)
            {
                return BadRequest(new
                {
                    mensagem = "O administrador não pode excluir a própria conta."
                });
            }

            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            bool possuiProdutos = await _context.Produtos
                .AnyAsync(p => p.IdUsuarioCadastro == id ||
                               p.IdUsuarioUpdate == id);

            if (possuiProdutos)
            {
                return Conflict(
                    "O usuário possui vínculos com produtos. " +
                    "Inative-o em vez de excluí-lo.");
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}