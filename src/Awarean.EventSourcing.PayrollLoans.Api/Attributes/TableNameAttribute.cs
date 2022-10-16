namespace Awarean.EventSourcing.PayrollLoans.Api.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class TableNameAttribute : Attribute
{
    public string TableName { get; }
    public TableNameAttribute(string tableName)
    {
        ArgumentNullException.ThrowIfNull(tableName);
        if (string.IsNullOrWhiteSpace(tableName)) throw new ArgumentException("Property TableName cannot be null, empty string or a white space.");
        if (tableName == string.Empty) throw new ArgumentException("Property TableName cannot be null, empty string or a white space.");

        TableName = tableName;
    }
}
