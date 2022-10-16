using Awarean.EventSourcing.PayrollLoans.Api.Attributes;

namespace Awarean.EventSourcing.PayrollLoans.Api.Extensions;

public static class TypeExtensions
{
    public static string GetTableName(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        var attributes = type.GetCustomAttributes(inherit: true);

        var tableAttr = attributes.Single(x => x is TableNameAttribute) as TableNameAttribute;

        return tableAttr.TableName 
            ?? throw new ArgumentException(
                $"{type.Name} does not contain a {nameof(TableNameAttribute)} with the name for the table to be used in queries");
    }
}
