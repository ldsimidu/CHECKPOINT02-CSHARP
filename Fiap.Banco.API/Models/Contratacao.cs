using System.ComponentModel.DataAnnotations;

namespace Fiap.Banco.API.Models
{
    public class Contratacao
    {
        [Key]
        public int idContratacao { get; set; }

        public int clienteId { get; set; }

        public int produtoId { get; set; }

        public DateTime dataSolicitacao { get; set; } = DateTime.UtcNow;

        public string status { get; set; } = "Pendente";

        public string observacao { get; set; } = string.Empty;

        public Cliente? Cliente { get; set; }

        public Produto? Produto { get; set; }
    }
}
