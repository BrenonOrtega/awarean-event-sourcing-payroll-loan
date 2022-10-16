using Awarean.EventSourcing.PayrollLoans.Api.Entities;
using Awarean.EventSourcing.PayrollLoans.Api.Entities.PayrollLoans.Commands;
using Awarean.Sdk.Result;

namespace Awarean.EventSourcing.PayrollLoans.Api.Services.Abstractions;
public interface IPayrollLoanService
{
    Task<Result> CreateLoan(CreateLoanCommand command);
    Task<Result> PayInstallments(PayInstallmentCommand command);
    Task<Result<PayrollLoan>> GetLoan(Guid id);
    #if DEBUG
    Task<IEnumerable<PayrollLoan>> GetAllAsync();
    #endif
}
