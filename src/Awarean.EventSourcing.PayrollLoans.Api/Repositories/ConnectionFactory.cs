using System.Data;
using Awarean.EventSourcing.PayrollLoans.Api.Repositories.Abstractions;

namespace Awarean.EventSourcing.PayrollLoans.Api.Repositories;

internal class ConnectionFactory : IConnectionFactory
{
    private readonly IDbConnection _connection;

    public ConnectionFactory(IDbConnection connection)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    public IDbConnection GetPersistence() => _connection;
}