using Microsoft.AspNetCore.Mvc;
using MiniErp.Domain.Entities;
using MiniErp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MiniErp.Api.Dtos;

namespace MiniErp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoResponse>>> Listar()
        {
            var produtos = await _context.Produtos.ToListAsync();

            var response = produtos.Select(p => new ProdutoResponse
            {
                Id = p.Id,
                Sku = p.Sku,
                Nome = p.Nome,
                PrecoVenda = p.PrecoVenda,
                EstoqueAtual = p.EstoqueAtual,
                Ativo = p.Ativo,
                CategoriaId = p.CategoriaId,
                FornecedorId = p.FornecedorId,

            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoResponse>> BuscarPorId(int id)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null)
            {
                return NotFound();
            }

            var response = new ProdutoResponse
            {
                Id = produto.Id,
                Sku = produto.Sku,
                Nome = produto.Nome,
                PrecoVenda = produto.PrecoVenda,
                EstoqueAtual = produto.EstoqueAtual,
                Ativo = produto.Ativo,
                CategoriaId = produto.CategoriaId,
                FornecedorId = produto.FornecedorId
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoResponse>> CriarProduto(CriarProdutoRequest request)
        {
            var produto = new Produto
            {
                Sku = request.Sku,
                Nome = request.Nome,
                PrecoVenda = request.PrecoVenda,
                EstoqueAtual = 0,
                Ativo = request.Ativo,
                CategoriaId = request.CategoriaId,
                FornecedorId = request.FornecedorId
            };

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            var response = new ProdutoResponse
            {
                Id = produto.Id,
                Sku = produto.Sku,
                Nome = produto.Nome,
                PrecoVenda = produto.PrecoVenda,
                EstoqueAtual = produto.EstoqueAtual,
                Ativo = produto.Ativo,
                CategoriaId = produto.CategoriaId,
                FornecedorId = produto.FornecedorId
            };

            return CreatedAtAction(nameof(BuscarPorId), new { id = produto.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarProduto(int id, AtualizarProdutoRequest request)
        {

            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);

            if(produto == null)
            {
                return NotFound();
            }

            produto.Sku = request.Sku;
            produto.Nome = request.Nome;
            produto.PrecoVenda = request.PrecoVenda;
            produto.Ativo = request.Ativo;
            produto.CategoriaId = request.CategoriaId;
            produto.FornecedorId = request.FornecedorId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarProduto(int id)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);

            if(produto == null)
            {
                return NotFound();
            }

            if (await _context.MovimentacoesEstoque.AnyAsync(m => m.ProdutoId == id))
            {
                return Conflict(
                    "Não é possível excluir um produto com histórico de movimentações.");
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
};
