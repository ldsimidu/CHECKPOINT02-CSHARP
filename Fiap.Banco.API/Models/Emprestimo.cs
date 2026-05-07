namespace Fiap.Banco.API.Models
{
    public class Emprestimo : Produto
    {
        public decimal valorSolicitado { get; set; }

        public decimal taxaJurosMensal { get; set; }

        public int quantidadeParcelas { get; set; }
    }
}
