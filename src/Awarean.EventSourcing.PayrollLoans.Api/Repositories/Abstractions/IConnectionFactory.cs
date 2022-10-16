using System.Data;

namespace Awarean.EventSourcing.PayrollLoans.Api.Repositories.Abstractions;

public interface IConnectionFactory
{
    IDbConnection GetPersistence();
}
