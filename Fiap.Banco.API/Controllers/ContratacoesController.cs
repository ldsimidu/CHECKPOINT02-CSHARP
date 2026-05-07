using Fiap.Banco.API.Data;
using Fiap.Banco.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.idProduto == request.produtoId && p.ativo);
            if (produto == null)
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
