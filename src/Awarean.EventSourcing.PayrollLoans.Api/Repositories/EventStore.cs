using System.Runtime.InteropServices;
using Awarean.EventSourcing.PayrollLoans.Api.Configurations;
using Awarean.EventSourcing.PayrollLoans.Api.Entities.Base;
using Awarean.EventSourcing.PayrollLoans.Api.Extensions;
using Awarean.EventSourcing.PayrollLoans.Api.Repositories.Abstractions;
using Awarean.Sdk.Result;
using Dapper;
using Microsoft.Extensions.Options;

namespace Awarean.EventSourcing.PayrollLoans.Api.Repositories;

internal class EventStore : IEventStore
{
    private readonly IConnectionFactory _factory;
    private readonly ILogger<EventStore> _logger;
    private readonly EventStoreOptions _options;

    public EventStore(IConnectionFactory factory, IOptions<EventStoreOptions> options, ILogger<EventStore> logger)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result> CommitEvent<TId, TEntity>(Event<TId, TEntity> @event)
    {
        var sql = _options.CommitEventQuery;
        DynamicParameters parameters = new DynamicParameters();

        parameters.Add(sql.Parameters[nameof(@event.EntityId)], @event.EntityId);
        parameters.Add(sql.Parameters[nameof(@event.CreatedAt)], @event.CreatedAt);
        parameters.Add(sql.Parameters[nameof(@event.EntityType)], @event.EntityType);
        parameters.Add(sql.Parameters[nameof(@event.EventType)], @event.EventType);
        parameters.Add(sql.Parameters[nameof(@event.Version)], @event.Version);
        parameters.Add(sql.Parameters[nameof(@event.SerializedEvent)], @event.SerializedEvent);

        try
        {
            using var conn = _factory.GetPersistence();
            await conn.ExecuteAsync(sql.QueryTable(@event.GetType().GetTableName()),
             parameters);
        }
        catch (ExternalException ee)
        {
            if (ee.Message.Contains("duplicate key"))
            {
                return Result.Fail("DUPLICATED_OPERATION_ERROR", $"A duplicated event for the entity id {@event.EntityId} was incorrectly issued.");
            }

            throw;
        }
        catch (Exception ex)
        {
            return Result.Fail("PERSISTENCE_ERROR", $"An error ocurred while persisting event. Inner Exception: {ex.Message}");
        }

        return Result.Success();
    }

    public async Task<Result<IEnumerable<Event<TId, TEntity>>>> GetEvents<TId, TEntity>(TId id)
    {
        var tableName = typeof(TEntity).GetEventTable();
        var sql = _options.GetEventsQuery;
        using var conn = _factory.GetPersistence();
        var parameters = new DynamicParameters();
        parameters.Add(sql.Parameters[nameof(id)], id);

        var queried = Enumerable.Empty<Event<TId, TEntity>>();

        try
        {
            queried = await conn.QueryAsync<Event<TId, TEntity>>(
                sql: sql.QueryTable(tableName),
                param: parameters);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception happened during {method} execution, exception {exception}", "GetEvents<,>", ex);

            return Result.Fail<IEnumerable<Event<TId, TEntity>>>(
                code: "FAILED_QUERYING_EVENTS",
                reason: $"An error ocurred while querying table {tableName} - error : {ex.Message}");
        }

        var returned = queried ?? Enumerable.Empty<Event<TId, TEntity>>();
        return Result.Success(queried);
    }
}
