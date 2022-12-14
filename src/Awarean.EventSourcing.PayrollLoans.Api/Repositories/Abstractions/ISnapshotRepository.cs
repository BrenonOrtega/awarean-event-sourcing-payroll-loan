namespace Awarean.EventSourcing.PayrollLoans.Api.Repositories.Abstractions;

public interface ISnapshotRepository<in TId, TEntity>
{
    Task<TEntity> GetByIdAsync(TId id);
    
    #if DEBUG    
    Task<IEnumerable<TEntity>> GetAllAsync(int index, int pageSize);
    #endif
}
