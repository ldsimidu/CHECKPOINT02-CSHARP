namespace Fiap.BancoDigital.API.Dtos;

public class CriarAgenciaDto
{
    public string Numero { get; set; } = string.Empty;

    public string Nome { get; set; } = string.Empty;

    public string Cidade { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;
}

public class AgenciaResponseDto
{
    public int Id { get; set; }

    public string Numero { get; set; } = string.Empty;

    public string Nome { get; set; } = string.Empty;

    public string Cidade { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;
}