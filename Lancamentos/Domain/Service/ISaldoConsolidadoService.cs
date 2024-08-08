namespace Lancamentos.Domain.Service;

public interface ISaldoConsolidadoService
{
    Task<List<SaldoConsolidadoEntity>> GetSaldoConsolidado();
}
