namespace Awarean.EventSourcing.PayrollLoans.Api.Entities.PayrollLoans.Commands;

public class CreateLoanCommand
{
    public Guid  Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public string DocumentNumber { get; set; }
    public int InstallmentNumber { get; set; }
    public double InterestRate { get; set; }
    public int ExpectedVersion { get; set; }
}
