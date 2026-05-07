using Fiap.Banco.API.Data;
using Fiap.Banco.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Banco.API.Controllers
{
    [Route("api/produtos")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("emprestimo")]
        public async Task<ActionResult<Emprestimo>> PostEmprestimo([FromBody] CadastrarEmprestimoRequest request)
        {
            var produto = new Emprestimo
            {
                nomeProduto = request.nomeProduto,
                ativo = request.ativo,
                valorSolicitado = request.valorSolicitado,
                taxaJurosMensal = request.taxaJurosMensal,
                quantidadeParcelas = request.quantidadeParcelas
            };

            _context.Emprestimos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduto), new { id = produto.idProduto }, produto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.idProduto == id);
            if (produto == null)
            {
                return NotFound();
            }

            return Ok(produto);
        }
    }

    public record CadastrarEmprestimoRequest(
        string nomeProduto,
        bool ativo,
        decimal valorSolicitado,
        decimal taxaJurosMensal,
        int quantidadeParcelas);
}
