namespace Lancamentos.Domain.Service;

public interface ILancamentoService
{
    Task<Lancamento> Create(Lancamento lancamento);
    Task<Lancamento> GetById (Guid id);
    Task<List<Lancamento>> GetAll();
    Task<Lancamento> Update(Lancamento lancamento);
    Task Delete(Guid id);
}
