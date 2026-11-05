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

            if (produto == null)
            {
                return NotFound();
            }

            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> CriarProduto(Produto produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(BuscarPorId), new { id = produto.Id }, produto);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> AutalizarPrtoduto(int id, Produto ProdutoAtualizado)
        {
            if(id != ProdutoAtualizado.Id)
            {
                return BadRequest("O ID da URL deve ser igual ao ID informado no corpo.");
            }

            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);

            if(produto == null)
            {
                return NotFound();
            }

            produto.Sku = ProdutoAtualizado.Sku;
            produto.Nome = ProdutoAtualizado.Nome;
            produto.PrecoVenda = ProdutoAtualizado.PrecoVenda;
            produto.EstoqueAtual = ProdutoAtualizado.EstoqueAtual;
            produto.Ativo = ProdutoAtualizado.Ativo;

            await _context.SaveChangesAsync();

            return NoContent();
        }




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
