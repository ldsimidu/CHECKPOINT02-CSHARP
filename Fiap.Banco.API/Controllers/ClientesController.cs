using Fiap.Banco.API.Data;
using Fiap.Banco.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Banco.API.Controllers
{
    [Route("api/clientes")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("pf")]
        public async Task<ActionResult<PessoaFisica>> PostPessoaFisica([FromBody] CadastrarPessoaFisicaRequest request)
        {
            var agenciaExiste = await _context.Agencias.AnyAsync(a => a.idAgencia == request.agenciaId);
            if (!agenciaExiste)
            {
                return NotFound($"Agência {request.agenciaId} não encontrada.");
            }

            var cpfJaExiste = await _context.PessoasFisicas.AnyAsync(p => p.cpf == request.cpf);
            if (cpfJaExiste)
            {
                return Conflict("CPF já cadastrado.");
            }

            var cliente = new PessoaFisica
            {
                nome = request.nome,
                agenciaId = request.agenciaId,
                cpf = request.cpf,
                dataNascimento = request.dataNascimento
            };

            _context.PessoasFisicas.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.id }, cliente);
        }

        [HttpPost("pj")]
        public async Task<ActionResult<PessoaJuridica>> PostPessoaJuridica([FromBody] CadastrarPessoaJuridicaRequest request)
        {
            var agenciaExiste = await _context.Agencias.AnyAsync(a => a.idAgencia == request.agenciaId);
            if (!agenciaExiste)
            {
                return NotFound($"Agência {request.agenciaId} não encontrada.");
            }

            var cnpjJaExiste = await _context.PessoasJuridicas.AnyAsync(p => p.cnpj == request.cnpj);
            if (cnpjJaExiste)
            {
                return Conflict("CNPJ já cadastrado.");
            }

            var cliente = new PessoaJuridica
            {
                nome = request.nome,
                agenciaId = request.agenciaId,
                cnpj = request.cnpj,
                razaoSocial = request.razaoSocial
            };

            _context.PessoasJuridicas.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.id }, cliente);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Agencia)
                .FirstOrDefaultAsync(c => c.id == id);

            if (cliente == null)
            {
                return NotFound();
            }

            return Ok(cliente);
        }
    }

    public record CadastrarPessoaFisicaRequest(string nome, int agenciaId, string cpf, DateTime dataNascimento);
    public record CadastrarPessoaJuridicaRequest(string nome, int agenciaId, string cnpj, string razaoSocial);
}
