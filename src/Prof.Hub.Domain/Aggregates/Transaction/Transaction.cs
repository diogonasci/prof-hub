using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Transaction.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Transaction;
public class Transaction : AuditableEntity, IAggregateRoot
{
    public TransactionId Id { get; private set; }
    public Money Amount { get; private set; }
    public TransactionType Type { get; private set; }
    public string Description { get; private set; }

    private Transaction(TransactionId id, Money amount, TransactionType type, string description)
    {
        Id = id;
        Amount = amount;
        Type = type;
        Description = description;
        Created = DateTime.UtcNow;
    }

    public static Result<Transaction> Create(Money amount, TransactionType type, string description)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(description))
            errors.Add(new ValidationError("A descrição da transação é obrigatória"));

        if (errors.Count != 0)
            return Result.Invalid(errors);

        return new Transaction(TransactionId.Create(), amount, type, description);
    }
}
