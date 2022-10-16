namespace Awarean.EventSourcing.PayrollLoans.Api.Configurations;

public class SnapshotOptions
{
    public SqlQuery GetById { get; init; }
    public SqlQuery GetAll { get; init; }
}
