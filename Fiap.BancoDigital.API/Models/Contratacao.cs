using Fiap.BancoDigital.API.Enums;

namespace Fiap.BancoDigital.API.Models;

public class Contratacao
{
    public int Id { get; set; }

    public int ClienteId { get; set; }

    public Cliente? Cliente { get; set; }

    public int ProdutoId { get; set; }

    public Produto? Produto { get; set; }

    public DateTime DataSolicitacao { get; set; } = DateTime.Now;

    public DateTime? DataProcessamento { get; set; }

    public StatusContratacao Status { get; set; } = StatusContratacao.Pendente;

    public string? MensagemResultado { get; set; }
}