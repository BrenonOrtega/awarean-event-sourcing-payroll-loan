using Awarean.EventSourcing.PayrollLoans.Api.Entities.Base;

namespace Awarean.EventSourcing.PayrollLoans.Api.Entities.PayrollLoans.Events;

public class InstallmentPaidEvent : PayrollLoanEvent
{
    public InstallmentPaidEvent(PayrollLoan loan, int paidInstallments)
    {
        EntityId = loan.Id;
        CreatedAt = loan.UpdatedAt;
        PaidAmount = loan.InstallmentAmount;
        PaidInstallments = paidInstallments;
    }

    protected InstallmentPaidEvent() { }

    public decimal PaidAmount { get; init; }
    public int PaidInstallments { get; init; }
}
