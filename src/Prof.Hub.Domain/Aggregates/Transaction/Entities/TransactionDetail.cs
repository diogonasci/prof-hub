using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Transaction.ValueObjects;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Transaction.Entities;
public class TransactionDetail : Entity
{
    public TransactionDetailId Id { get; private set; }
    public TransactionId TransactionId { get; private set; }
    public TransactionDetailType Type { get; private set; }
    public string Description { get; private set; }
    public Money Amount { get; private set; }
    public string? Reference { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private TransactionDetail() { }

    public static Result<TransactionDetail> Create(
        TransactionId transactionId,
        TransactionDetailType type,
        string description,
        Money amount,
        string? reference = null)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Result.Invalid(new ValidationError("Descrição é obrigatória"));

        var detail = new TransactionDetail
        {
            Id = TransactionDetailId.Create(),
            TransactionId = transactionId,
            Type = type,
            Description = description,
            Amount = amount,
            Reference = reference,
            CreatedAt = DateTime.UtcNow
        };

        return detail;
    }
}
