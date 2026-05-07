using Microsoft.EntityFrameworkCore;

namespace Fiap.Banco.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options ) : base( options ) { }

        public DbSet<Models.Banco> Bancos { get; set; } 

        public DbSet<Models.Cliente> Clientes { get; set; } 
        public DbSet<Models.PessoaFisica> PessoasFisicas { get; set; }
        public DbSet<Models.PessoaJuridica> PessoasJuridicas { get; set; }
        public DbSet<Models.Produto> Produtos { get; set; }
        public DbSet<Models.Emprestimo> Emprestimos { get; set; }
        public DbSet<Models.MaquinaDeCartao> MaquinasDeCartao { get; set; }
        public DbSet<Models.ReceberSalario> RecebimentosSalario { get; set; }
        public DbSet<Models.Contratacao> Contratacoes { get; set; }

        public DbSet<Models.Funcionario> Funcionarios { get; set; } 

        public DbSet<Models.Agencia> Agencias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Models.Cliente>()
                .ToTable("AgenciaClientes")
                .HasDiscriminator<string>("TipoCliente")
                .HasValue<Models.PessoaFisica>("PF")
                .HasValue<Models.PessoaJuridica>("PJ");

            modelBuilder.Entity<Models.Produto>()
                .HasDiscriminator<string>("TipoProduto")
                .HasValue<Models.Emprestimo>("Emprestimo")
                .HasValue<Models.MaquinaDeCartao>("MaquinaDeCartao")
                .HasValue<Models.ReceberSalario>("ReceberSalario");

            modelBuilder.Entity<Models.Produto>(entity =>
            {
                entity.Property(p => p.ativo)
                    .HasConversion(
                        v => v ? 1 : 0,
                        v => v == 1)
                    .HasColumnType("NUMBER(1)");
            });

            modelBuilder.Entity<Models.Emprestimo>(entity =>
            {
                entity.Property(p => p.valorSolicitado).HasPrecision(18, 2);
                entity.Property(p => p.taxaJurosMensal).HasPrecision(5, 2);
            });

            modelBuilder.Entity<Models.MaquinaDeCartao>(entity =>
            {
                entity.Property(p => p.aluguelMensal).HasPrecision(18, 2);
                entity.Property(p => p.taxaMdrPercentual).HasPrecision(5, 2);
            });

            modelBuilder.Entity<Models.ReceberSalario>(entity =>
            {
                entity.Property(p => p.limiteAntecipacaoPercentual).HasPrecision(5, 2);
            });

            modelBuilder.Entity<Models.Contratacao>(entity =>
            {
                entity.Property(c => c.status).HasMaxLength(30);
                entity.Property(c => c.observacao).HasMaxLength(500);

                entity.HasOne(c => c.Cliente)
                    .WithMany()
                    .HasForeignKey(c => c.clienteId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Produto)
                    .WithMany()
                    .HasForeignKey(c => c.produtoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
