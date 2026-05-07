using Fiap.Banco.API.Data;
using Fiap.Banco.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Fiap.Banco.API.Controllers
{
    [Route("api/contratacoes")]
    [ApiController]
    public class ContratacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContratacoesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Contratacao>> PostContratacao([FromBody] SolicitarContratacaoRequest request)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.id == request.clienteId);
            if (cliente == null)
            {
                return NotFound($"Cliente {request.clienteId} não encontrado.");
            }

            var connection = _context.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            await using var command = connection.CreateCommand();
            command.CommandText = @"SELECT COUNT(1) FROM ""Produtos"" WHERE ""idProduto"" = :produtoId AND ""ativo"" = 1";

            var parametro = command.CreateParameter();
            parametro.ParameterName = "produtoId";
            parametro.Value = request.produtoId;
            command.Parameters.Add(parametro);

            var scalar = await command.ExecuteScalarAsync();
            var produtoAtivoCount = Convert.ToInt32(scalar ?? 0);
            if (produtoAtivoCount == 0)
            {
                return NotFound($"Produto {request.produtoId} não encontrado ou inativo.");
            }

            var contratacao = new Contratacao
            {
                clienteId = request.clienteId,
                produtoId = request.produtoId,
                status = "EmAnalise",
                observacao = "Solicitação recebida para processamento."
            };

            _context.Contratacoes.Add(contratacao);
            await _context.SaveChangesAsync();

            return AcceptedAtAction(nameof(GetContratacao), new { id = contratacao.idContratacao }, contratacao);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contratacao>> GetContratacao(int id)
        {
            var contratacao = await _context.Contratacoes
                .Include(c => c.Cliente)
                .Include(c => c.Produto)
                .FirstOrDefaultAsync(c => c.idContratacao == id);

            if (contratacao == null)
            {
                return NotFound();
            }

            return Ok(contratacao);
        }
    }

    public record SolicitarContratacaoRequest(int clienteId, int produtoId);
}
