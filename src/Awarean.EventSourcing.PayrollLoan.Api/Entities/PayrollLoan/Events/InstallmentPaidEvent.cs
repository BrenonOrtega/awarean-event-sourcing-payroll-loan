using Awarean.EventSourcing.PayrollLoans.Api.Entities.Base;

namespace Awarean.EventSourcing.PayrollLoans.Api.Entities.PayrollLoan.Events;

public class InstallmentPaidEvent : Event<Guid, PayrollLoan>
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
