using Fiap.BancoDigital.API.Data;
using Fiap.BancoDigital.API.Dtos;
using Fiap.BancoDigital.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiap.BancoDigital.API.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClientesController : ControllerBase
{
    private readonly BancoDbContext _context;

    public ClientesController(BancoDbContext context)
    {
        _context = context;
    }

    [HttpPost("pf")]
    public async Task<ActionResult<ClienteResponseDto>> CriarPessoaFisica(CriarPessoaFisicaDto dto)
    {
        if (!await _context.Agencias.AnyAsync(a => a.Id == dto.AgenciaId))
        {
            return NotFound(new { mensagem = "Agência informada não existe." });
        }

        var cpf = ApenasNumeros(dto.Cpf);

        if (cpf.Length != 11)
        {
            return BadRequest(new { mensagem = "CPF deve conter 11 números." });
        }

        var cpfJaExiste = await _context.PessoasFisicas.AnyAsync(p => p.Cpf == cpf);

        if (cpfJaExiste)
        {
            return Conflict(new { mensagem = "CPF já cadastrado." });
        }

        var cliente = new PessoaFisica
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Telefone = dto.Telefone,
            AgenciaId = dto.AgenciaId,
            Cpf = cpf,
            DataNascimento = dto.DataNascimento
        };

        _context.PessoasFisicas.Add(cliente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarClientePorId), new { id = cliente.Id }, MapearCliente(cliente));
    }

    [HttpPost("pj")]
    public async Task<ActionResult<ClienteResponseDto>> CriarPessoaJuridica(CriarPessoaJuridicaDto dto)
    {
        if (!await _context.Agencias.AnyAsync(a => a.Id == dto.AgenciaId))
        {
            return NotFound(new { mensagem = "Agência informada não existe." });
        }

        var cnpj = ApenasNumeros(dto.Cnpj);

        if (cnpj.Length != 14)
        {
            return BadRequest(new { mensagem = "CNPJ deve conter 14 números." });
        }

        var cnpjJaExiste = await _context.PessoasJuridicas.AnyAsync(p => p.Cnpj == cnpj);

        if (cnpjJaExiste)
        {
            return Conflict(new { mensagem = "CNPJ já cadastrado." });
        }

        var cliente = new PessoaJuridica
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Telefone = dto.Telefone,
            AgenciaId = dto.AgenciaId,
            Cnpj = cnpj,
            RazaoSocial = dto.RazaoSocial
        };

        _context.PessoasJuridicas.Add(cliente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarClientePorId), new { id = cliente.Id }, MapearCliente(cliente));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClienteResponseDto>> BuscarClientePorId(int id)
    {
        var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);

        if (cliente == null)
        {
            return NotFound(new { mensagem = "Cliente não encontrado." });
        }

        return Ok(MapearCliente(cliente));
    }

    private static ClienteResponseDto MapearCliente(Cliente cliente)
    {
        if (cliente is PessoaFisica pf)
        {
            return new ClienteResponseDto
            {
                Id = pf.Id,
                Tipo = "PF",
                Nome = pf.Nome,
                Email = pf.Email,
                Telefone = pf.Telefone,
                AgenciaId = pf.AgenciaId,
                Cpf = pf.Cpf,
                DataNascimento = pf.DataNascimento
            };
        }

        if (cliente is PessoaJuridica pj)
        {
            return new ClienteResponseDto
            {
                Id = pj.Id,
                Tipo = "PJ",
                Nome = pj.Nome,
                Email = pj.Email,
                Telefone = pj.Telefone,
                AgenciaId = pj.AgenciaId,
                Cnpj = pj.Cnpj,
                RazaoSocial = pj.RazaoSocial
            };
        }

        throw new InvalidOperationException("Tipo de cliente inválido.");
    }

    private static string ApenasNumeros(string valor)
    {
        return new string(valor.Where(char.IsDigit).ToArray());
    }
}