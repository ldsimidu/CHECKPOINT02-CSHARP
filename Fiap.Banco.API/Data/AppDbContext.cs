using Microsoft.EntityFrameworkCore;

namespace Fiap.Banco.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options ) : base( options ) { }

        public DbSet<Models.Banco> Bancos { get; set; } 

        public DbSet<Models.Cliente> Clientes { get; set; } 

        public DbSet<Models.Funcionario> Funcionarios { get; set; }

        public DbSet<Models.Agencia> Agencias { get; set; }
    }
}
