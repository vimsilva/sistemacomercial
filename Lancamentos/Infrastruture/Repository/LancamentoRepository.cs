using Lancamentos.Domain;
using Lancamentos.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Lancamentos.Infrastruture.Repository;

public class LancamentoRepository : ILancamentoRepository
{
    private readonly LancamentoContext _context;
    public LancamentoRepository(LancamentoContext context)
    {
        _context = context;
    }
    public async Task Delete(Guid id)
    {
        var lancamento = await _context.Lancamentos.FindAsync(id);
        if (lancamento != null)
        {
            _context.Lancamentos.Remove(lancamento);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Lancamento>> FindAll()
    {
        return await _context.Lancamentos.ToListAsync();
    }

    public async Task<Lancamento> FindById(Guid id)
    {
        return await _context.Lancamentos.FindAsync(id);
    }

    public async Task Save(Lancamento lancamento)
    {
        await _context.Lancamentos.AddAsync(lancamento);
        var teste = await _context.SaveChangesAsync();
        Console.WriteLine("Fim");
    }

    public async Task Update(Lancamento lancamento)
    {
        _context.Lancamentos.Update(lancamento);
        await _context.SaveChangesAsync();
    }
}
