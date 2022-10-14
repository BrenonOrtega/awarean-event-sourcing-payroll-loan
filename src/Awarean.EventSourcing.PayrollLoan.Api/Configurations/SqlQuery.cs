using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Awarean.EventSourcing.PayrollLoans.Api.Configurations
{
    public class SqlQuery
    {
        public string Query { get; init; }
        public Dictionary<string, string> Parameters { get; init; } = new();
    }
}