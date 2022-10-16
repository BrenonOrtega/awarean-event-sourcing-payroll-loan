using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Awarean.EventSourcing.PayrollLoans.Api.Attributes;
using Awarean.EventSourcing.PayrollLoans.Api.Entities.Base;

namespace Awarean.EventSourcing.PayrollLoans.Api.Entities.PayrollLoans.Events
{
    [TableName("payroll_loans_events")]
    public abstract class PayrollLoanEvent : Event<Guid, PayrollLoan>
    {
        
    }
}