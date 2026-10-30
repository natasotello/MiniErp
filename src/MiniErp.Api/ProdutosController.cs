using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MiniErp.Domain.Entities;

namespace MiniErp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private static readonly List<Produto> _produtos = new()
    {
        new Produto { Id = 1, Sku = "REFRI-2L", Nome = "Refrigerante 2L", PrecoVenda = 9.90m, EstoqueAtual = 50, Ativo = true }
    };

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Listar()
        {
            return Ok(_produtos);
        }

        [HttpGet("{id}")]
        public ActionResult<Produto> BuscarPorId(int id)
        {
            var produto = _produtos.FirstOrDefault(p => p.Id == id);

            if (produto == null)
            {
                return NotFound();
            }

            return Ok(produto);
        }

        [HttpPost]
        public ActionResult<Produto> CriarProduto(Produto produto)
        {
            produto.Id = _produtos.Max(p => p.Id) + 1;
            _produtos.Add(produto);

            return CreatedAtAction(nameof(BuscarPorId), new { id = produto.Id }, produto);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarProduto(int id, Produto produtoAtualizado)
        {
            var produto = _produtos.FirstOrDefault(p => p.Id == id);

            if(produto == null)
            {
                return NotFound();
            }

            produto.Sku = produtoAtualizado.Sku;
            produto.Nome = produtoAtualizado.Nome;
            produto.PrecoVenda = produtoAtualizado.PrecoVenda;
            produto.EstoqueAtual = produtoAtualizado.EstoqueAtual;
            produto.Ativo = produtoAtualizado.Ativo;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarProduto(int id)
        {
            var produto = _produtos.FirstOrDefault(p => p.Id == id);

            if(produto == null)
            {
                return NotFound();
            }

            _produtos.Remove(produto);

            return NoContent();
        }
    }
};
