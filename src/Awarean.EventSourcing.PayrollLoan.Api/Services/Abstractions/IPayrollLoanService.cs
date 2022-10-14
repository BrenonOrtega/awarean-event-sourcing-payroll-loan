using Awarean.EventSourcing.PayrollLoans.Api.Entities.PayrollLoan.Commands;
using Awarean.Sdk.Result;
using Loan = Awarean.EventSourcing.PayrollLoans.Api.Entities.PayrollLoan.PayrollLoan;

namespace Awarean.EventSourcing.PayrollLoans.Api.Services.Abstractions;
public interface IPayrollLoanService
{
    Task<Result> CreateLoan(CreateLoanCommand command);
    Task<Result> PayInstallments(PayInstallmentCommand command);
    Task<Result<Loan>> GetLoan(Guid id);
}
