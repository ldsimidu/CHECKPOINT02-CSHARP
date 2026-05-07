using System.ComponentModel.DataAnnotations;

namespace Fiap.Banco.API.Models
{
    public abstract class Produto
    {
        [Key]
        public int idProduto { get; set; }

        public string nomeProduto { get; set; } = string.Empty;

        public bool ativo { get; set; }
    }
}
