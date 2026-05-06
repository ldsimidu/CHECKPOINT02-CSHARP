namespace Fiap.BancoDigital.API.Dtos;

public class CriarPessoaFisicaDto
{
    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public int AgenciaId { get; set; }

    public string Cpf { get; set; } = string.Empty;

    public DateTime DataNascimento { get; set; }
}

public class CriarPessoaJuridicaDto
{
    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public int AgenciaId { get; set; }

    public string Cnpj { get; set; } = string.Empty;

    public string RazaoSocial { get; set; } = string.Empty;
}

public class ClienteResponseDto
{
    public int Id { get; set; }

    public string Tipo { get; set; } = string.Empty;

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public int AgenciaId { get; set; }

    public string? Cpf { get; set; }

    public DateTime? DataNascimento { get; set; }

    public string? Cnpj { get; set; }

    public string? RazaoSocial { get; set; }
}