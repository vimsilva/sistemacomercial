using Lancamentos.Domain;
using Lancamentos.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Lancamentos.Infrastruture.Repository;

public class SaldoConsolidadoRepository : ISaldoConsolidadoRepository
{

    private readonly SaldoConsolidadoContext _context;
    public SaldoConsolidadoRepository(SaldoConsolidadoContext context)
    {
        _context = context;
    }
    public async Task<List<SaldoConsolidadoEntity>> FindAll()
    {
        return await _context.SaldosConsolidados.ToListAsync();
    }
}
