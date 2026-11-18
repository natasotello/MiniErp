using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniErp.Api.Dtos;
using MiniErp.Application.Services;
using MiniErp.Domain.Entities;
using MiniErp.Infrastructure.Data;

namespace MiniErp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentacoesEstoqueController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly EstoqueService _estoqueService;

        public MovimentacoesEstoqueController(AppDbContext context, EstoqueService estoqueService)
        {
            _context = context;
            _estoqueService = estoqueService;
        }

        [HttpGet("produto/{produtoId:int}")]
        public async Task<ActionResult<IEnumerable<MovimentacaoResponse>>> ListarPorProduto(int produtoId)
        {
            var movimentacoes = await _context.MovimentacoesEstoque
                .Where(m => m.ProdutoId == produtoId)
                .OrderByDescending(m => m.DataHora)
                .Select(m => new MovimentacaoResponse
                {
                    Id = m.Id,
                    ProdutoId = m.ProdutoId,
                    Tipo = m.Tipo,
                    Quantidade = m.Quantidade,
                    DataHora = m.DataHora,
                    Observacao = m.Observacao
                })
                .ToListAsync();

            return Ok(movimentacoes);
        }

        [HttpPost]
        public async Task<ActionResult<MovimentacaoResponse>> Registrar(CriarMovimentacaoRequest request)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == request.ProdutoId);

            if(produto == null)
            {
                return NotFound("Produto não encontrado.");
            }

            MovimentacaoEstoque movimentacao;

            try
            {
                movimentacao = _estoqueService.CriarMovimentacao(
                    produto,
                    request.Tipo,
                    request.Quantidade,
                    request.Observacao);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }

            _context.MovimentacoesEstoque.Add(movimentacao);

            await _context.SaveChangesAsync();

            var response = new MovimentacaoResponse
            {
                Id = movimentacao.Id,
                ProdutoId = movimentacao.ProdutoId,
                Tipo = movimentacao.Tipo,
                Quantidade = movimentacao.Quantidade,
                DataHora = movimentacao.DataHora,
                Observacao = movimentacao.Observacao
            };

            return StatusCode(201, response);
        }
    }
}
