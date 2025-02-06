using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Transaction.Events;
using Prof.Hub.Domain.Aggregates.Transaction.ValueObjects;
using Prof.Hub.Domain.Aggregates.Wallet.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Transaction;
public class Transaction : AuditableEntity, IAggregateRoot
{
    private const int MAX_DESCRIPTION_LENGTH = 200;

    public TransactionId Id { get; private set; }
    public WalletId WalletId { get; private set; }
    public Money Amount { get; private set; }
    public TransactionType Type { get; private set; }
    public TransactionSource Source { get; private set; }
    public string Description { get; private set; }
    public string? ExternalReference { get; private set; }

    private Transaction(
        TransactionId id,
        WalletId walletId,
        Money amount,
        TransactionType type,
        TransactionSource source,
        string description,
        string? externalReference = null)
    {
        Id = id;
        WalletId = walletId;
        Amount = amount;
        Type = type;
        Source = source;
        Description = description;
        ExternalReference = externalReference;
        Created = DateTime.UtcNow;
    }

    public static Result<Transaction> Create(
        WalletId walletId,
        Money amount,
        TransactionType type,
        TransactionSource source,
        string description,
        string? externalReference = null)
    {
        var errors = new List<ValidationError>();

        if (walletId == null)
            errors.Add(new ValidationError("Wallet é obrigatória"));

        if (amount == null || amount.Amount <= 0)
            errors.Add(new ValidationError("Valor da transação deve ser maior que zero"));

        if (string.IsNullOrWhiteSpace(description))
            errors.Add(new ValidationError("Descrição da transação é obrigatória"));

        if (description?.Length > MAX_DESCRIPTION_LENGTH)
            errors.Add(new ValidationError($"Descrição não pode exceder {MAX_DESCRIPTION_LENGTH} caracteres"));

        if (!IsValidTransactionType(type, source))
            errors.Add(new ValidationError("Tipo de transação inválido para esta origem"));

        if (RequiresExternalReference(source) && string.IsNullOrWhiteSpace(externalReference))
            errors.Add(new ValidationError("Referência externa é obrigatória para este tipo de transação"));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        var transaction = new Transaction(
            TransactionId.Create(),
            walletId,
            amount,
            type,
            source,
            description,
            externalReference);

        transaction.AddDomainEvent(new TransactionCreatedEvent(transaction.WalletId, transaction.Id, transaction.Amount));

        return transaction;
    }

    private static bool IsValidTransactionType(TransactionType type, TransactionSource source)
    {
        return (source, type) switch
        {
            (TransactionSource.ClassPayment, TransactionType.Debit) => true,
            (TransactionSource.ClassRefund, TransactionType.Credit) => true,
            (TransactionSource.ReferralBonus, TransactionType.ReferralBonus) => true,
            (TransactionSource.WalletCredit, TransactionType.Credit) => true,
            _ => false
        };
    }

    private static bool RequiresExternalReference(TransactionSource source)
    {
        return source == TransactionSource.ClassPayment || source == TransactionSource.ClassRefund;
    }
}
