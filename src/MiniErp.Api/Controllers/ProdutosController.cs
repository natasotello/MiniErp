using Microsoft.AspNetCore.Mvc;
using MiniErp.Domain.Entities;
using MiniErp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<Produto>>> Listar()
        {
            var produtos = await _context.Produtos.ToListAsync();

            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> BuscarPorId(int id)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);

            if(produto == null)
            {
                return NotFound();
            }

            return Ok(produto);
        }

        //[HttpPost]
        //public ActionResult<Produto> CriarProduto(Produto produto)
        //{
        //    produto.Id = _produtos.Max(p => p.Id) + 1;
        //    _produtos.Add(produto);

        //    return CreatedAtAction(nameof(BuscarPorId), new { id = produto.Id }, produto);
        //}

        //[HttpPut("{id}")]
        //public IActionResult AtualizarProduto(int id, Produto produtoAtualizado)
        //{
        //    var produto = _produtos.FirstOrDefault(p => p.Id == id);

        //    if(produto == null)
        //    {
        //        return NotFound();
        //    }

        //    produto.Sku = produtoAtualizado.Sku;
        //    produto.Nome = produtoAtualizado.Nome;
        //    produto.PrecoVenda = produtoAtualizado.PrecoVenda;
        //    produto.EstoqueAtual = produtoAtualizado.EstoqueAtual;
        //    produto.Ativo = produtoAtualizado.Ativo;

        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public IActionResult DeletarProduto(int id)
        //{
        //    var produto = _produtos.FirstOrDefault(p => p.Id == id);

        //    if(produto == null)
        //    {
        //        return NotFound();
        //    }

        //    _produtos.Remove(produto);

        //    return NoContent();
        //}
    }
};
