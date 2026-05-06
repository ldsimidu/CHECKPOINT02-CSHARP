namespace Fiap.BancoDigital.API.Dtos;

public class SolicitarContratacaoEmprestimoDto
{
    public int ClienteId { get; set; }

    public decimal ValorSolicitado { get; set; }

    public int QuantidadeParcelas { get; set; }

    public decimal RendaMensalDeclarada { get; set; }
}

public class ContratacaoResponseDto
{
    public int Id { get; set; }

    public int ClienteId { get; set; }

    public int ProdutoId { get; set; }

    public string Produto { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime DataSolicitacao { get; set; }

    public DateTime? DataProcessamento { get; set; }

    public string? MensagemResultado { get; set; }
}