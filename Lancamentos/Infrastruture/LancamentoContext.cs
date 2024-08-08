using Lancamentos.Domain;
using Microsoft.EntityFrameworkCore;

namespace Lancamentos.Infrastruture;

public class LancamentoContext : DbContext
{
    public LancamentoContext(DbContextOptions<LancamentoContext> options) : base(options) { }

    public DbSet<Lancamento> Lancamentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Lancamento>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Tipo)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.Valor)
                .IsRequired()
                .HasPrecision(15,2);
            entity.Property(e => e.DataLancamento)
                .IsRequired();
            entity.Property(e => e.DataCriacao)
                .IsRequired();
            entity.Property(e => e.DataAtualizacao)
                .IsRequired();
        });
    }
}
