using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniErp.Api.Dtos;
using MiniErp.Domain.Entities;
using MiniErp.Infrastructure.Data;

namespace MiniErp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaResponse>>> Listar()
        {
            var categorias = await _context.Categorias.ToListAsync();

            var response = categorias.Select(c => new CategoriaResponse
            {
                Id = c.Id,
                Nome = c.Nome,
                Descricao = c.Descricao,
                Ativa = c.Ativa

            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaResponse>> BuscarPorId(int id)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);

            if(categoria == null)
            {
                return NotFound();
            }

            var response = new CategoriaResponse
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                Descricao = categoria.Descricao,
                Ativa = categoria.Ativa

            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaResponse>> CriarCategoria(CriarCategoriaRequest request)
        {
            var categoria = new Categoria
            {
                Nome = request.Nome,
                Descricao = request.Descricao,
                Ativa = request.Ativa

            };

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            var response = new CategoriaResponse
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                Descricao = categoria.Descricao,
                Ativa = categoria.Ativa

            };

            return CreatedAtAction(nameof(BuscarPorId), new { id = categoria.Id }, response);
        }
    }
}
