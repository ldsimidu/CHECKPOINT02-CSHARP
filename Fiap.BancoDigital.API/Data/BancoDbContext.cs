using Fiap.BancoDigital.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Fiap.BancoDigital.API.Data;

public class BancoDbContext : DbContext
{
    public BancoDbContext(DbContextOptions<BancoDbContext> options) : base(options)
    {
    }

    public DbSet<Agencia> Agencias => Set<Agencia>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<PessoaFisica> PessoasFisicas => Set<PessoaFisica>();
    public DbSet<PessoaJuridica> PessoasJuridicas => Set<PessoaJuridica>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Emprestimo> Emprestimos => Set<Emprestimo>();
    public DbSet<Contratacao> Contratacoes => Set<Contratacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Agencia>(entity =>
        {
            entity.ToTable("PB_AGENCIAS");
            entity.HasKey(a => a.Id);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("PB_CLIENTES");

            entity.HasDiscriminator<string>("TIPO_CLIENTE")
                .HasValue<PessoaFisica>("PF")
                .HasValue<PessoaJuridica>("PJ");

            entity.HasOne(c => c.Agencia)
                .WithMany(a => a.Clientes)
                .HasForeignKey(c => c.AgenciaId);
        });

        modelBuilder.Entity<PessoaFisica>(entity =>
        {
            entity.Property(p => p.Cpf).HasMaxLength(11);
            entity.HasIndex(p => p.Cpf).IsUnique();
        });

        modelBuilder.Entity<PessoaJuridica>(entity =>
        {
            entity.Property(p => p.Cnpj).HasMaxLength(14);
            entity.HasIndex(p => p.Cnpj).IsUnique();
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.ToTable("PB_PRODUTOS");

            entity.HasDiscriminator<string>("TIPO_PRODUTO")
                .HasValue<Emprestimo>("EMPRESTIMO");
        });

        modelBuilder.Entity<Emprestimo>(entity =>
        {
            entity.Property(e => e.ValorSolicitado).HasPrecision(18, 2);
            entity.Property(e => e.RendaMensalDeclarada).HasPrecision(18, 2);
            entity.Property(e => e.TaxaJurosMensal).HasPrecision(10, 4);
        });

        modelBuilder.Entity<Contratacao>(entity =>
        {
            entity.ToTable("PB_CONTRATACOES");

            entity.Property(c => c.Status)
                .HasConversion<string>()
                .HasMaxLength(30);

            entity.HasOne(c => c.Cliente)
                .WithMany(c => c.Contratacoes)
                .HasForeignKey(c => c.ClienteId);

            entity.HasOne(c => c.Produto)
                .WithMany()
                .HasForeignKey(c => c.ProdutoId);
        });
    }
}