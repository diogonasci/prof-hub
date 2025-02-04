using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
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
    public IReadOnlyList<Transaction.Transaction> Transactions => _transactions.AsReadOnly();

    private Wallet(WalletId id, StudentId studentId, Money balance)
    {
        Id = id;
        StudentId = studentId;
        Balance = balance;
    }

    public static Result<Wallet> Create(StudentId studentId)
    {
        var balanceResult = Money.Create(0);
        if (!balanceResult.IsSuccess)
            return Result<Wallet>.Invalid(balanceResult.ValidationErrors);

        return new Wallet(WalletId.Create(), studentId, balanceResult.Value);
    }

    public Result AddTransaction(Transaction.Transaction transaction)
    {
        var errors = new List<ValidationError>();

        if (transaction.Type == TransactionType.Debit && Balance.Amount < transaction.Amount.Amount)
            errors.Add(new ValidationError("Saldo insuficiente para realizar a transação"));

        if (errors.Count != 0)
            return Result.Invalid(errors);

        _transactions.Add(transaction);

        Balance = transaction.Type switch
        {
            TransactionType.Credit or TransactionType.Refund or TransactionType.ReferralBonus
                => Money.Create(Balance.Amount + transaction.Amount.Amount, Balance.Currency),
            TransactionType.Debit
                => Money.Create(Balance.Amount - transaction.Amount.Amount, Balance.Currency),
            _ => Balance
        };

        return Result.Success();
    }
}
