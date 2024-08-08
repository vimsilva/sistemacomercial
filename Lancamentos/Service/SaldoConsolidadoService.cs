using Lancamentos.Domain;
using Lancamentos.Domain.Repository;
using Lancamentos.Domain.Service;
using Microsoft.Extensions.Options;

namespace Lancamentos.Service;

public class SaldoConsolidadoService : ISaldoConsolidadoService
{

    private readonly ISaldoConsolidadoRepository _repository;

    public SaldoConsolidadoService(ISaldoConsolidadoRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<SaldoConsolidadoEntity>> GetSaldoConsolidado()
    {
        return await _repository.FindAll();
    }
}
