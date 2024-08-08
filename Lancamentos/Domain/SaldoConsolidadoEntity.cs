namespace Lancamentos.Domain;

public class SaldoConsolidadoEntity
{
    public Guid Id { get; set; }
    public DateTime Data { get; set; }
    public decimal Saldo { get; set; }
}
