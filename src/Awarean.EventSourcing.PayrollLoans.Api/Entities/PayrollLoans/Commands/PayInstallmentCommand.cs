
namespace Awarean.EventSourcing.PayrollLoans.Api.Entities.PayrollLoans.Commands;
public class PayInstallmentCommand
{
    public Guid LoanId { get; set; }
    public decimal Amount { get; set; }
    public int Installments { get; set; }
    public int ExpectedVersion { get; internal set; }
}
