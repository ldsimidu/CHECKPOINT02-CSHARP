using Fiap.BancoDigital.API.Data;
using Fiap.BancoDigital.API.Dtos;
using Fiap.BancoDigital.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiap.BancoDigital.API.Controllers;

[ApiController]
[Route("api/agencias")]
public class AgenciasController : ControllerBase
{
    private readonly BancoDbContext _context;

    public AgenciasController(BancoDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<AgenciaResponseDto>> CriarAgencia(CriarAgenciaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Numero) ||
            string.IsNullOrWhiteSpace(dto.Nome) ||
            string.IsNullOrWhiteSpace(dto.Cidade) ||
            string.IsNullOrWhiteSpace(dto.Estado))
        {
            return BadRequest(new { mensagem = "Todos os campos da agência são obrigatórios." });
        }

        var agencia = new Agencia
        {
            Numero = dto.Numero,
            Nome = dto.Nome,
            Cidade = dto.Cidade,
            Estado = dto.Estado.ToUpper()
        };

        _context.Agencias.Add(agencia);
        await _context.SaveChangesAsync();

        var response = new AgenciaResponseDto
        {
            Id = agencia.Id,
            Numero = agencia.Numero,
            Nome = agencia.Nome,
            Cidade = agencia.Cidade,
            Estado = agencia.Estado
        };

        return CreatedAtAction(nameof(BuscarAgenciaPorId), new { id = agencia.Id }, response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AgenciaResponseDto>> BuscarAgenciaPorId(int id)
    {
        var agencia = await _context.Agencias.FirstOrDefaultAsync(a => a.Id == id);

        if (agencia == null)
        {
            return NotFound(new { mensagem = "Agência não encontrada." });
        }

        return Ok(new AgenciaResponseDto
        {
            Id = agencia.Id,
            Numero = agencia.Numero,
            Nome = agencia.Nome,
            Cidade = agencia.Cidade,
            Estado = agencia.Estado
        });
    }
}