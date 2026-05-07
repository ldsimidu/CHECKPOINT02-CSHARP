using System.ComponentModel.DataAnnotations;

namespace Fiap.Banco.API.Models
{
    public class Banco
    {

        [Key] 
        public int idBanco { get; set; }

        public string nomeBanco { get; set; }

        public DateTime dtCriacao { get; set; }
    }
}
