namespace Fiap.Banco.API.Models
{
    public class ReceberSalario : Produto
    {
        public string cnpjConvenio { get; set; } = string.Empty;

        public decimal limiteAntecipacaoPercentual { get; set; }
    }
}
