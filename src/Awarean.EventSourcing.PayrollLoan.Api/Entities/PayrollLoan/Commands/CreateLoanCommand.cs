namespace Awarean.EventSourcing.PayrollLoans.Api.Entities.PayrollLoan.Commands;

public class CreateLoanCommand
{
    public Guid  Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public int MyProperty { get; set; }
    public string DocumentNumber { get; internal set; }
    public int InstallmentNumber { get; internal set; }
    public double InterestRate { get; internal set; }
    public int ExpectedVersion { get; internal set; }
}
