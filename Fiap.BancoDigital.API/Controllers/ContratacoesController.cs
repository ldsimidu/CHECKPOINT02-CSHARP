using Fiap.BancoDigital.API.Data;
using Fiap.BancoDigital.API.Dtos;
using Fiap.BancoDigital.API.Enums;
using Fiap.BancoDigital.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiap.BancoDigital.API.Controllers;

[ApiController]
[Route("api/contratacoes")]
public class ContratacoesController : ControllerBase
{
    private readonly BancoDbContext _context;

    public ContratacoesController(BancoDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<ContratacaoResponseDto>> SolicitarContratacao(SolicitarContratacaoEmprestimoDto dto)
    {
        var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == dto.ClienteId);

        if (!clienteExiste)
        {
            return NotFound(new { mensagem = "Cliente não encontrado." });
        }

        if (dto.ValorSolicitado <= 0)
        {
            return BadRequest(new { mensagem = "Valor solicitado deve ser maior que zero." });
        }

        if (dto.QuantidadeParcelas <= 0)
        {
            return BadRequest(new { mensagem = "Quantidade de parcelas deve ser maior que zero." });
        }

        if (dto.RendaMensalDeclarada <= 0)
        {
            return BadRequest(new { mensagem = "Renda mensal declarada deve ser maior que zero." });
        }

        var emprestimo = new Emprestimo
        {
            Nome = "Empréstimo Pessoal",
            Descricao = "Produto bancário de empréstimo com análise de crédito.",
            ValorSolicitado = dto.ValorSolicitado,
            QuantidadeParcelas = dto.QuantidadeParcelas,
            RendaMensalDeclarada = dto.RendaMensalDeclarada,
            TaxaJurosMensal = 0,
            ScoreCalculado = 0
        };

        _context.Emprestimos.Add(emprestimo);
        await _context.SaveChangesAsync();

        var contratacao = new Contratacao
        {
            ClienteId = dto.ClienteId,
            ProdutoId = emprestimo.Id,
            Status = StatusContratacao.EmAnalise,
            DataSolicitacao = DateTime.Now
        };

        ProcessarEmprestimo(contratacao, emprestimo);

        _context.Contratacoes.Add(contratacao);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarContratacaoPorId), new { id = contratacao.Id }, MapearContratacao(contratacao, emprestimo));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ContratacaoResponseDto>> BuscarContratacaoPorId(int id)
    {
        var contratacao = await _context.Contratacoes
            .Include(c => c.Produto)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (contratacao == null)
        {
            return NotFound(new { mensagem = "Contratação não encontrada." });
        }

        return Ok(MapearContratacao(contratacao, contratacao.Produto));
    }

    private static void ProcessarEmprestimo(Contratacao contratacao, Emprestimo emprestimo)
    {
        var valorParcelaSemJuros = emprestimo.ValorSolicitado / emprestimo.QuantidadeParcelas;
        var comprometimentoRenda = valorParcelaSemJuros / emprestimo.RendaMensalDeclarada;

        var score = CalcularScore(emprestimo, comprometimentoRenda);

        emprestimo.ScoreCalculado = score;

        if (score >= 650 && comprometimentoRenda <= 0.35m)
        {
            emprestimo.TaxaJurosMensal = 0.025m;
            contratacao.Status = StatusContratacao.Aprovada;
            contratacao.MensagemResultado = $"Empréstimo aprovado. Score calculado: {score}. Taxa mensal: 2,5%.";
        }
        else
        {
            emprestimo.TaxaJurosMensal = 0;
            contratacao.Status = StatusContratacao.Recusada;
            contratacao.MensagemResultado = $"Empréstimo recusado. Score calculado: {score}. Comprometimento de renda acima do permitido ou score insuficiente.";
        }

        contratacao.DataProcessamento = DateTime.Now;
    }

    private static int CalcularScore(Emprestimo emprestimo, decimal comprometimentoRenda)
    {
        var score = 900;

        if (emprestimo.RendaMensalDeclarada < 1500)
        {
            score -= 250;
        }

        if (emprestimo.ValorSolicitado > emprestimo.RendaMensalDeclarada * 12)
        {
            score -= 200;
        }

        if (comprometimentoRenda > 0.35m)
        {
            score -= 250;
        }
        else if (comprometimentoRenda > 0.25m)
        {
            score -= 100;
        }

        if (emprestimo.QuantidadeParcelas > 36)
        {
            score -= 100;
        }

        return Math.Max(score, 0);
    }

    private static ContratacaoResponseDto MapearContratacao(Contratacao contratacao, Produto? produto)
    {
        return new ContratacaoResponseDto
        {
            Id = contratacao.Id,
            ClienteId = contratacao.ClienteId,
            ProdutoId = contratacao.ProdutoId,
            Produto = produto?.Nome ?? "Empréstimo Pessoal",
            Status = contratacao.Status.ToString(),
            DataSolicitacao = contratacao.DataSolicitacao,
            DataProcessamento = contratacao.DataProcessamento,
            MensagemResultado = contratacao.MensagemResultado
        };
    }
}