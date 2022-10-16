using Awarean.EventSourcing.PayrollLoans.Api.Attributes;
using Awarean.EventSourcing.PayrollLoans.Api.Configurations;
using Awarean.EventSourcing.PayrollLoans.Api.Extensions;
using Awarean.EventSourcing.PayrollLoans.Api.Repositories.Abstractions;
using Dapper;
using Microsoft.Extensions.Options;

namespace Awarean.EventSourcing.PayrollLoans.Api.Repositories;

internal class SnapshotRepository<TId, TEntity> : ISnapshotRepository<TId, TEntity> where TEntity : class
{
    private readonly IConnectionFactory _factory;
    private readonly SnapshotOptions _Sql;
    private readonly string _tableName;

    public SnapshotRepository(IConnectionFactory factory, IOptions<SnapshotOptions> Sql) 
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _Sql = Sql?.Value ?? throw new ArgumentNullException(nameof(Sql));
        _tableName = typeof(TEntity).GetTableName();
    }

    public Task<IEnumerable<TEntity>> GetAllAsync(int index, int pageSize)
    {
        var sql = _Sql.GetAll;
        var offset = (index <= 0 ? 1 : index) * pageSize;

        var parameters = NewDynamicParams();
        parameters.Add(sql.Parameters[nameof(offset)], offset);
        parameters.Add(sql.Parameters[nameof(pageSize)], pageSize);

        using var conn = _factory.GetPersistence();

        return conn.QueryAsync<TEntity>(sql.Query,
            new {@OFFSET = offset, @FETCH = pageSize });
    }

    public async Task<TEntity> GetByIdAsync(TId id)
    {
        var sql = _Sql.GetById ?? throw new ArgumentNullException(nameof(_Sql.GetById));
        using var conn = _factory.GetPersistence();
        DynamicParameters parameters = NewDynamicParams();
        parameters.Add(sql.Parameters[nameof(id)], id);

        var queryResult = await conn.QuerySingleOrDefaultAsync<TEntity>(sql .Query, parameters);

        return queryResult ?? typeof(TEntity).GetField("Empty").GetValue(null) as TEntity;
    }

    private DynamicParameters NewDynamicParams()
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TABLE", _tableName);
        return parameters;
    }
}
