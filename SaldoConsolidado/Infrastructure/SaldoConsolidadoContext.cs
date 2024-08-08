using Microsoft.EntityFrameworkCore;
using SaldoConsolidado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaldoConsolidado.Infrastructure;
public class SaldoConsolidadoContext : DbContext
{
    public SaldoConsolidadoContext(DbContextOptions<SaldoConsolidadoContext> options) : base(options) { }

    public DbSet<SaldoConsolidadoEntity> SaldosConsolidados { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SaldoConsolidadoEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Data).IsRequired();
            entity.Property(e => e.Saldo).IsRequired().HasColumnType("decimal(15,2)");
        });
    }
}
