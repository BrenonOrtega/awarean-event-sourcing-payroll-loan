
namespace Awarean.EventSourcing.PayrollLoans.Api.Configurations;

public class EventStoreOptions
{
    public SqlQuery CommitEventQuery { get; init; }
    public SqlQuery GetEventsQuery { get; init; }
}
