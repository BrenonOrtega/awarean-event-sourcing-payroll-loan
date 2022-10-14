using Awarean.EventSourcing.PayrollLoans.Api.Configurations;
using Awarean.EventSourcing.PayrollLoans.Api.Repositories.Abstractions;
using Dapper;

namespace Awarean.EventSourcing.PayrollLoans.Api.Repositories;

internal class SnapshotRepository<TId, TEntity> : ISnapshotRepository<TId, TEntity> where TEntity : class
{
    private readonly IConnectionFactory _factory;
    private readonly SqlQuery _Sql;

    public SnapshotRepository(IConnectionFactory factory, SqlQuery Sql) 
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _Sql = Sql ?? throw new ArgumentNullException(nameof(Sql));
    }
    
    public async Task<TEntity> GetByIdAsync(TId id)
    {
        using var conn = _factory.GetPersistence();
        var parameters = new DynamicParameters();
        parameters.Add(_Sql.Parameters[nameof(id)], id);

        var queryResult = await conn.QuerySingleOrDefaultAsync<TEntity>(_Sql.Query, parameters);

        return queryResult ?? typeof(TEntity).GetField("Empty").GetValue(null) as TEntity;
    }
}
