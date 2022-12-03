using Awarean.EventSourcing.PayrollLoans.Api.Entities;
using Awarean.EventSourcing.PayrollLoans.Api.Entities.PayrollLoans.Commands;
using Awarean.EventSourcing.PayrollLoans.Api.Repositories.Abstractions;
using Awarean.EventSourcing.PayrollLoans.Api.Services.Abstractions;
using Awarean.Sdk.Result;

namespace Awarean.EventSourcing.PayrollLoans.Api.Services;

public class PayrollLoanService : IPayrollLoanService
{
    private readonly IEventStore _eventStore;
    private readonly ISnapshotRepository<Guid, PayrollLoan> _snapshot;

    public PayrollLoanService(IEventStore eventStore, ISnapshotRepository<Guid, PayrollLoan> snapshot)
    {
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        _snapshot = snapshot ?? throw new ArgumentNullException(nameof(snapshot));
    }

    public async Task<Result<PayrollLoan>> GetLoan(Guid id)
    {
        var result = await _eventStore.GetEvents<Guid, PayrollLoan>(id);

        if (result.IsFailed)
            return Result.Fail<PayrollLoan>(result.Error);

        var events = result.Value;
        dynamic loan = Activator.CreateInstance<PayrollLoan>();

        foreach (var @event in events)
        {
            var obj = Activator.CreateInstance(
                $"{nameof(Awarean)}.{nameof(EventSourcing)}.{nameof(PayrollLoans)}.{nameof(Api)}",
                @event.EventType,
                @event.GetType()
                    .GetProperties()
                    .Select(x => x.GetValue(@event))
                    .ToArray()
            );

            var instance = Convert.ChangeType(obj?.Unwrap(), Type.GetType(@event.EntityType));

            loan.Apply(instance);
        }

        return Result.Success<PayrollLoan>(loan);
    }

    public async Task<Result> CreateLoan(CreateLoanCommand command)
    {
        var loan = await _snapshot.GetByIdAsync(command.Id);

        if (loan != PayrollLoan.Empty)
            return Result.Fail("LOAN_EXISTS", "There is already a loan with given Id, please try again with another id");

        var newLoanResult = PayrollLoan.Create(command);

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
        var result = await _eventStore.GetEvents<Guid, PayrollLoan>(command.LoanId);

        if (result.IsFailed)
            return Result.Fail("LOAN_DOES_NOT_EXISTS", 
                "There is not a loan with given Id, please try again with an existing id");
        
        var events = result.Value;
        if (events.Any(x => x.Version >= command.ExpectedVersion))
            return Result.Fail("EXPECTED_VERSION_DUPLICATED",
                "There is already an command taking place in this operatin, please verify if it's not duplicated.");    


        var loan = PayrollLoan.Build(events);
        var paidResult = loan.Apply(command);

        if (paidResult.IsSuccess)
            await _eventStore.CommitEvent(paidResult.Value);

        return result;
    }

    #if DEBUG
    public Task<IEnumerable<PayrollLoan>> GetAllAsync()
    {
        return _snapshot.GetAllAsync(0, int.MaxValue);
    }
    #endif
}
