using Awarean.EventSourcing.PayrollLoans.Api.Entities.PayrollLoan.Commands;
using Awarean.EventSourcing.PayrollLoans.Api.Repositories.Abstractions;
using Awarean.EventSourcing.PayrollLoans.Api.Services.Abstractions;
using Awarean.Sdk.Result;
using Loan = Awarean.EventSourcing.PayrollLoans.Api.Entities.PayrollLoan.PayrollLoan;

namespace Awarean.EventSourcing.PayrollLoans.Api.Services;

public class PayrollLoanService : IPayrollLoanService
{
    private readonly IEventStore _eventStore;
    private readonly ISnapshotRepository<Guid, Loan> _snapshot;

    public PayrollLoanService(IEventStore eventStore, ISnapshotRepository<Guid, Loan> snapshot)
    {
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        _snapshot = snapshot ?? throw new ArgumentNullException(nameof(snapshot));
    }

    public async Task<Result<Loan>> GetLoan(Guid id)
    {
        var result = await _eventStore.GetEvents<Guid, Loan>(id);

        if (result.IsFailed)
            return Result.Fail<Loan>(result.Error);

        var events = result.Value;
        dynamic loan = Activator.CreateInstance<Loan>();

        foreach (var @event in events)
        {
            var obj = Activator.CreateInstance(
                "Awarean.EventSourcing.PayrollLoans.Api",
                @event.Type,
                @event.GetType()
                    .GetProperties()
                    .Select(x => x.GetValue(@event))
                    .ToArray()
            );

            var instance = Convert.ChangeType(obj?.Unwrap(), Type.GetType(@event.Type));

            loan.Apply(instance);
        }

        return Result.Success<Loan>(loan);
    }

    public async Task<Result> CreateLoan(CreateLoanCommand command)
    {
        var loan = await _snapshot.GetByIdAsync(command.Id);

        if (loan is not null || loan == Loan.Empty)
            return Result.Fail("LOAN_EXISTS", "There is already a loan with given Id, please try again with another id");

        var newLoanResult = Loan.Create(command);

        if (newLoanResult.IsFailed)
            return newLoanResult;

        var newLoan = newLoanResult.Value;
        var commitResult = await _eventStore.CommitEvent(newLoan.Events[0]);

        if (commitResult.IsFailed)
            return commitResult;

        return Result.Success(newLoan);
    }

    public async Task<Result> PayInstallments(PayInstallmentCommand command)
    {
        var loan = await _snapshot.GetByIdAsync(command.LoanId);

        if (loan is null || loan == Loan.Empty)
            return Result.Fail("LOAN_DOES_NOT_EXISTS", "There is not a loan with given Id, please try again with an existing id");

        var result = loan.Apply(command);

        if (result.IsSuccess)
            await _eventStore.CommitEvent(result.Value);

        return result;
    }
}
