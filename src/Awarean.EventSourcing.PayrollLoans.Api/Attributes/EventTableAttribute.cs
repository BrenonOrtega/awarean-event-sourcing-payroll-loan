namespace Awarean.EventSourcing.PayrollLoans.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class EventTableAttribute : TableNameAttribute
    {
        public EventTableAttribute(string tableName) : base(tableName)
        {
        }
    }
}
