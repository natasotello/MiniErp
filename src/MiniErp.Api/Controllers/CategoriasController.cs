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
    }
}
