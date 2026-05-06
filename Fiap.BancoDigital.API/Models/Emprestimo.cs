namespace Fiap.BancoDigital.API.Models;

public class Emprestimo : Produto
{
    public decimal ValorSolicitado { get; set; }

    public int QuantidadeParcelas { get; set; }

    public decimal RendaMensalDeclarada { get; set; }

    public decimal TaxaJurosMensal { get; set; }

    public int ScoreCalculado { get; set; }
}