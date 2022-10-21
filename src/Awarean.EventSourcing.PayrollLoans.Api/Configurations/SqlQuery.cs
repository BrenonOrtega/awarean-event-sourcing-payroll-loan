namespace Awarean.EventSourcing.PayrollLoans.Api.Configurations;

public class SqlQuery
{
    public string Query { get; init; }
    public string QueryTable(string tableName) => string.Format(Query, tableName);
    public Dictionary<string, string> Parameters { get; init; } = new();
}
