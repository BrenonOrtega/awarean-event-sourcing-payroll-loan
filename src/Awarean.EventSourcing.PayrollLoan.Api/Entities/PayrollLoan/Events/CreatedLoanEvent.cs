using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Awarean.EventSourcing.PayrollLoans.Api.Entities.Base;

namespace Awarean.EventSourcing.PayrollLoans.Api.Entities.Loans.Events;

public class CreatedLoanEvent : Event<Guid, PayrollLoan>
{
    public CreatedLoanEvent(PayrollLoan loan)
    {
        EntityId = loan.Id;
        DocumentNumber = loan.DocumentNumber;
        CreatedAt = loan.CreatedAt;
        InterestRate = loan.InterestRate;
        LoanAmount = loan.Amount;
        FinalAmount = loan.FinalAmount;
        InstallmentNumber = loan.InstallmentNumber;
        Version = loan.LatestVersion is 1
            ? loan.LatestVersion
            : throw new ArgumentException(
                "Cannot instantiate an createdLoanEvent when the entity is not in first version");
    }

    protected CreatedLoanEvent() { }

    public string DocumentNumber { get; init; }
    public DateTime CreatedAt { get; init; }
    public double InterestRate { get; init; }
    public decimal LoanAmount { get; init; }
    public decimal FinalAmount { get; init; }
    public decimal Amount { get; init; }
    public int InstallmentNumber { get; init; }
}
