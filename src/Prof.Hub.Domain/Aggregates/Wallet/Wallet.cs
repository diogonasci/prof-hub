using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Aggregates.Wallet.Events;
using Prof.Hub.Domain.Aggregates.Wallet.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Wallet;
public class Wallet : AuditableEntity, IAggregateRoot
{
    private readonly List<Transaction.Transaction> _transactions = [];

    public WalletId Id { get; private set; }
    public StudentId StudentId { get; private set; }
    public Money Balance { get; private set; }
    public Money? DailyLimit { get; private set; }
    public Money? MonthlyLimit { get; private set; }
    public DateTime? LastTransactionDate { get; private set; }
    public IReadOnlyList<Transaction.Transaction> Transactions => _transactions.AsReadOnly();

    private Wallet(WalletId id, StudentId studentId, Money balance)
    {
        Id = id;
        StudentId = studentId;
        Balance = balance;
    }

    public static Result<Wallet> Create(StudentId studentId)
    {
        if (studentId == null)
            return Result.Invalid(new ValidationError("StudentId é obrigatório"));

        var balanceResult = Money.Create(0);

        if (!balanceResult.IsSuccess)
        {
            var errors = new List<ValidationError>();
            errors.AddRange(balanceResult.ValidationErrors);
            return Result.Invalid(errors);
        }

        var wallet = new Wallet(
            WalletId.Create(),
            studentId,
            balanceResult.Value
        );

        return wallet;
    }

    public Result AddTransaction(Transaction.Transaction transaction)
    {
        if (transaction == null)
            return Result.Invalid(new ValidationError("Transação é obrigatória"));

        var errors = new List<ValidationError>();

        if (_transactions.Any(t => t.Id == transaction.Id))
            errors.Add(new ValidationError("Transação duplicada"));

        if (transaction.Amount.Currency != Balance.Currency)
            errors.Add(new ValidationError("Moeda da transação diferente da carteira"));

        if (transaction.Type == TransactionType.Debit)
        {
            if (Balance.Amount < transaction.Amount.Amount)
                errors.Add(new ValidationError("Saldo insuficiente"));

            if (ExceedsDailyLimit(transaction.Amount))
                errors.Add(new ValidationError("Excede limite diário"));

            if (ExceedsMonthlyLimit(transaction.Amount))
                errors.Add(new ValidationError("Excede limite mensal"));
        }

        if (errors.Count > 0)
            return Result.Invalid(errors);

        _transactions.Add(transaction);

        var previousBalance = Balance;
        Balance = transaction.Type switch
        {
            TransactionType.Credit or TransactionType.Refund or TransactionType.ReferralBonus
                => Money.Create(Balance.Amount + transaction.Amount.Amount, Balance.Currency).Value,
            TransactionType.Debit
                => Money.Create(Balance.Amount - transaction.Amount.Amount, Balance.Currency).Value,
            _ => Balance
        };

        LastTransactionDate = DateTime.UtcNow;

        AddDomainEvent(new WalletTransactionAddedEvent(
            Id.Value,
            transaction.Id.Value,
            previousBalance.Amount,
            Balance.Amount,
            transaction.Amount.Amount,
            transaction.Type.ToString()
        ));

        return Result.Success();
    }

    public Result UpdateLimits(Money dailyLimit, Money monthlyLimit)
    {
        var errors = new List<ValidationError>();

        if (dailyLimit.Amount <= 0)
            errors.Add(new ValidationError("Limite diário deve ser maior que zero"));

        if (monthlyLimit.Amount <= 0)
            errors.Add(new ValidationError("Limite mensal deve ser maior que zero"));

        if (dailyLimit.Amount > monthlyLimit.Amount)
            errors.Add(new ValidationError("Limite diário não pode ser maior que o mensal"));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        DailyLimit = dailyLimit;
        MonthlyLimit = monthlyLimit;

        AddDomainEvent(new WalletLimitsUpdatedEvent(Id.Value, dailyLimit.Amount, monthlyLimit.Amount));

        return Result.Success();
    }

    public Result<Money> GetBalanceAt(DateTime date)
    {
        var transactions = _transactions.Where(t => t.Created <= date).ToList();
        var balanceResult = Money.Create(0, Balance.Currency);

        foreach (var transaction in transactions)
        {
            balanceResult = transaction.Type switch
            {
                TransactionType.Credit or TransactionType.Refund or TransactionType.ReferralBonus
                    => Money.Create(balanceResult.Value.Amount + transaction.Amount.Amount, Balance.Currency),
                TransactionType.Debit
                    => Money.Create(balanceResult.Value.Amount - transaction.Amount.Amount, Balance.Currency),
                _ => balanceResult
            };

            if (!balanceResult.IsSuccess)
                return Result.Invalid(balanceResult.ValidationErrors);
        }

        return balanceResult;
    }

    public IEnumerable<Transaction.Transaction> GetTransactionsByPeriod(DateTime start, DateTime end)
        => _transactions.Where(t => t.Created >= start && t.Created <= end).OrderByDescending(t => t.Created);

    private bool ExceedsDailyLimit(Money amount)
    {
        if (DailyLimit is null)
            return false;

        var today = DateTime.UtcNow.Date;
        var dailyTotal = _transactions
            .Where(t => t.Created.Date == today && t.Type == TransactionType.Debit)
            .Sum(t => t.Amount.Amount);

        return (dailyTotal + amount.Amount) > DailyLimit.Amount;
    }

    private bool ExceedsMonthlyLimit(Money amount)
    {
        if (MonthlyLimit is null)
            return false;

        var firstDayOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var monthlyTotal = _transactions
            .Where(t => t.Created >= firstDayOfMonth && t.Type == TransactionType.Debit)
            .Sum(t => t.Amount.Amount);

        return (monthlyTotal + amount.Amount) > MonthlyLimit.Amount;
    }
}
