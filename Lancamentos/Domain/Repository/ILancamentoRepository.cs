namespace Lancamentos.Domain.Repository;

public interface ILancamentoRepository
{
    Task Save(Lancamento lancamento);
    Task<Lancamento> FindById(Guid id);
    Task<List<Lancamento>> FindAll();
    Task Update(Lancamento lancamento);
    Task Delete(Guid id);
}
