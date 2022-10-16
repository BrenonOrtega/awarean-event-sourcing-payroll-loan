
using Awarean.EventSourcing.PayrollLoans.Api.Entities.Base;
using Awarean.Sdk.Result;

namespace Awarean.EventSourcing.PayrollLoans.Api.Repositories.Abstractions;

public interface IEventStore
{
    Task<Result> CommitEvent<TId, TEntity>(Event<TId, TEntity> @event);
    Task<Result<IEnumerable<Event<TId, TEntity>>>> GetEvents<TId, TEntity>(TId id);
}
