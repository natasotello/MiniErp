using Microsoft.AspNetCore.Mvc;
using MiniErp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MiniErp.Api.Dtos;
using MiniErp.Domain.Entities;

namespace MiniErp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FornecedoresController : ControllerBase
    {
        private readonly AppDbContext _context;
        public FornecedoresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FornecedorResponse>>> Listar()
        {
            var fornecedores = await _context.Fornecedores.ToListAsync();

            var response = fornecedores.Select(f => new FornecedorResponse
            {
                Id = f.Id,
                RazaoSocial = f.RazaoSocial,
                Cnpj = f.Cnpj,
                Email = f.Email,
                Telefone = f.Telefone,
                Ativo = f.Ativo
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FornecedorResponse>> BuscarPorId(int id)
        {
            var fornecedor = await _context.Fornecedores.FirstOrDefaultAsync(f => f.Id == id);

            if(fornecedor == null)
            {
                return NotFound();
            }

            var response = new FornecedorResponse
            {
                Id = fornecedor.Id,
                RazaoSocial = fornecedor.RazaoSocial,
                Cnpj = fornecedor.Cnpj,
                Email = fornecedor.Email,
                Telefone = fornecedor.Telefone,
                Ativo = fornecedor.Ativo
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<FornecedorResponse>> CriarFornecedor(CriarFornecedorRequest request)
        {
            var existe = await _context.Fornecedores.AnyAsync(f => f.Cnpj == request.Cnpj);

            if (existe)
            {
                return Conflict("Já existe um fornecedor cadastrado com este CNPJ.");
            }

            var fornecedor = new Fornecedor
            {
                RazaoSocial = request.RazaoSocial,
                Cnpj = request.Cnpj,
                Email = request.Email,
                Telefone = request.Telefone,
                Ativo = request.Ativo
            };

            _context.Fornecedores.Add(fornecedor);
            await _context.SaveChangesAsync();

            var response = new FornecedorResponse
            {
                Id = fornecedor.Id,
                RazaoSocial = fornecedor.RazaoSocial,
                Cnpj = fornecedor.Cnpj,
                Email = fornecedor.Email,
                Telefone = fornecedor.Telefone,
                Ativo = fornecedor.Ativo
            };

            return CreatedAtAction(nameof(BuscarPorId), new { id = fornecedor.Id }, response);
        }
    }
}
