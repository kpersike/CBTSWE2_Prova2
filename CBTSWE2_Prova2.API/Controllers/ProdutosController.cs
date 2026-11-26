/*
Nome: Kaik Persike Maiorquino
Prontuário: CB3029689

Nome: Luiz Gustavo Verissimo Monteiro
Prontuário: CB3030326
*/
using CBTSWE2_Prova2.API.Data;
using CBTSWE2_Prova2.Core;
using CBTSWE2_Prova2.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CBTSWE2_Prova2.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        private int ObterIdUsuario()
        {
            return int.Parse(
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier)!);
        }

        // GET: api/produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoResponseDTO>>> GetProdutos()
        {
            var produtos = await _context.Produtos
                .Select(p => new ProdutoResponseDTO
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Preco = p.Preco,
                    Status = p.Status,
                    IdUsuarioCadastro = p.IdUsuarioCadastro,
                    IdUsuarioUpdate = p.IdUsuarioUpdate
                })
                .ToListAsync();

            return Ok(produtos);
        }

        // GET: api/produtos/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoResponseDTO>> GetProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
                return NotFound("Produto não encontrado.");

            return Ok(new ProdutoResponseDTO
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Preco = produto.Preco,
                Status = produto.Status,
                IdUsuarioCadastro = produto.IdUsuarioCadastro,
                IdUsuarioUpdate = produto.IdUsuarioUpdate
            });
        }

        // POST: api/produtos
        [HttpPost]
        public async Task<ActionResult<ProdutoResponseDTO>> PostProduto(
            ProdutoCadastroDTO dto)
        {
            int usuarioId = ObterIdUsuario();

            var produto = new Produto
            {
                Nome = dto.Nome,
                Preco = dto.Preco,
                Status = dto.Status,
                IdUsuarioCadastro = usuarioId,
                IdUsuarioUpdate = usuarioId
            };

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            var response = new ProdutoResponseDTO
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Preco = produto.Preco,
                Status = produto.Status,
                IdUsuarioCadastro = produto.IdUsuarioCadastro,
                IdUsuarioUpdate = produto.IdUsuarioUpdate
            };

            return CreatedAtAction(
                nameof(GetProduto),
                new { id = produto.Id },
                response);
        }

        // PUT: api/produtos/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(
            int id, ProdutoUpdateDTO dto)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
                return NotFound("Produto não encontrado.");

            produto.Nome = dto.Nome;
            produto.Preco = dto.Preco;
            produto.Status = dto.Status;
            produto.IdUsuarioUpdate = ObterIdUsuario();

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/produtos/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
                return NotFound("Produto não encontrado.");

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}