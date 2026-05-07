using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiap.Banco.API.Models
{
    public class Cliente
    {
        [Key]
        public int id { get; set; }

        public string nome { get; set; }

        public int agenciaId { get; set; }

        [ForeignKey("agenciaId")]
        public Agencia? Agencia { get; set; }
    }
}
