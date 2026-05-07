namespace Fiap.Banco.API.Models
{
    public class PessoaFisica : Cliente
    {
        public string cpf { get; set; } = string.Empty;

        public DateTime dataNascimento { get; set; }
    }
}
