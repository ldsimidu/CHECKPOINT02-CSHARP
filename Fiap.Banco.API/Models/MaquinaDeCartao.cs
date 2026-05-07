namespace Fiap.Banco.API.Models
{
    public class MaquinaDeCartao : Produto
    {
        public decimal taxaMdrPercentual { get; set; }

        public decimal aluguelMensal { get; set; }
    }
}
