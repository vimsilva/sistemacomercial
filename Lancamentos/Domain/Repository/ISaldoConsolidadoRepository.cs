namespace Lancamentos.Domain.Repository;

public interface ISaldoConsolidadoRepository
{
    Task<List<SaldoConsolidadoEntity>> FindAll();
}
