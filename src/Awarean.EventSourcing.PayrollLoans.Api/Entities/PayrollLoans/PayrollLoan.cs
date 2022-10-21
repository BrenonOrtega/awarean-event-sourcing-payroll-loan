using System.Collections.Immutable;
using Awarean.EventSourcing.PayrollLoans.Api.Attributes;
using Awarean.EventSourcing.PayrollLoans.Api.Entities.Base;
using Awarean.EventSourcing.PayrollLoans.Api.Entities.PayrollLoans.Commands;
using Awarean.EventSourcing.PayrollLoans.Api.Entities.PayrollLoans.Events;
using Awarean.Sdk.Result;

namespace Awarean.EventSourcing.PayrollLoans.Api.Entities;

[TableName("payroll_loans")]
public class PayrollLoan
{
    private List<Event<Guid, PayrollLoan>> _events = new();

    public Guid Id { get; private set; }
    public string DocumentNumber { get; private set; }
    public int InstallmentNumber { get; private set; }
    public int PaidInstallments { get; private set; }
    public decimal Amount { get; private set; }
    public double InterestRate { get; private set; }
    public int LatestVersion { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private bool _canUpgradeVersion;

    public ImmutableList<Event<Guid, PayrollLoan>> Events { get => _events.ToImmutableList(); }
    public decimal FinalAmount { get => Amount * Convert.ToDecimal(Math.Pow((1 + InterestRate), InstallmentNumber)); }
    public decimal InstallmentAmount { get => FinalAmount / InstallmentNumber; }

    public static Result<PayrollLoan> Create(CreateLoanCommand command)
    {
        if (command is null)
            return Result.Fail<PayrollLoan>(KnownErrors.NullCommand);

        var loan = new PayrollLoan()
        {
            DocumentNumber = command.DocumentNumber,
            InstallmentNumber = command.InstallmentNumber,
            PaidInstallments = 0,
            Amount = command.Amount,
            InterestRate = command.InterestRate,
            CreatedAt = command.CreatedAt,
            UpdatedAt = command.CreatedAt,
            Id = command.Id != Guid.Empty ? command.Id : Guid.NewGuid()
        };

        loan.UpgradeVersion();
        loan.AppendEvent(new CreatedLoanEvent(loan));
        return Result.Success(loan);
    }

    public Result<InstallmentPaidEvent> Apply(PayInstallmentCommand command)
    {
        if (command is null)
            return Result.Fail<InstallmentPaidEvent>(KnownErrors.NullCommand);

        if (command.ExpectedVersion <= LatestVersion)
            return Result.Fail<InstallmentPaidEvent>("INCONSISTENT_VERSION", 
                "Received command is in a past version, please verify for duplicated commands or try again");

        if (command.Amount != InstallmentAmount * command.Installments)
            return Result.Fail<InstallmentPaidEvent>("INVALID_INSTALLMENT", 
                "The intended installment payment value is not a valid installment value");

        var @event = new InstallmentPaidEvent(this, command.Installments);
        PaidInstallments++;
        UpgradeVersion();
        AppendEvent(@event);
        return Result.Success(@event);
    }

    public Result<PayrollLoan> Apply(CreatedLoanEvent @event)
    {
        Id = @event.EntityId;
        DocumentNumber = @event.DocumentNumber;
        CreatedAt = @event.CreatedAt;
        UpdatedAt = @event.CreatedAt;
        InterestRate = @event.InterestRate;
        Amount = @event.Amount;
        InstallmentNumber = @event.InstallmentNumber;
        LatestVersion = @event.Version;

        return Result.Success(this);
    }

    public Result<PayrollLoan> Apply(InstallmentPaidEvent @event)
    {
        if (@event is null)
            return Result.Fail<PayrollLoan>(KnownErrors.NullCommand);

        if (Id != @event.EntityId)
            return Result.Fail<PayrollLoan>("DISTINCT_IDS", $"The received event with entityId {@event.EntityId} is not correlated to the entity {Id}");

        PaidInstallments += @event.PaidInstallments;
        UpdatedAt = @event.CreatedAt;

        return Result.Success(this);
    }

    public void UpgradeVersion()
    {
        LatestVersion += 1;
        UpdatedAt = DateTime.Now;
        _canUpgradeVersion = false;
    }

    public void AppendEvent(Event<Guid, PayrollLoan> @event)
    {
        _events.Add(@event);
        _canUpgradeVersion = true;
    }

    public static readonly PayrollLoan Empty = new PayrollLoan()
    {
        Id = Guid.Empty,
        CreatedAt = DateTime.MinValue,
        UpdatedAt = DateTime.MaxValue,
        Amount = -1,
        InstallmentNumber = -1,
        LatestVersion = 0,
        DocumentNumber = ""
    };
}
