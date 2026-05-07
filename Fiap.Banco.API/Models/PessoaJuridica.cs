namespace Fiap.Banco.API.Models
{
    public class PessoaJuridica : Cliente
    {
        public string cnpj { get; set; } = string.Empty;

        public string razaoSocial { get; set; } = string.Empty;
    }
}
