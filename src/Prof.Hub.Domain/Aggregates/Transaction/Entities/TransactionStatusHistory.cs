using Prof.Hub.Domain.Aggregates.Transaction.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Transaction.Entities;
public class TransactionStatusHistory : Entity
{
    public TransactionStatusHistoryId Id { get; private set; }
    public TransactionId TransactionId { get; private set; }
    public PaymentStatus OldStatus { get; private set; }
    public PaymentStatus NewStatus { get; private set; }
    public string? Reason { get; private set; }
    public DateTime ChangedAt { get; private set; }

    private TransactionStatusHistory() { }

    public static TransactionStatusHistory Create(
        TransactionId transactionId,
        PaymentStatus oldStatus,
        PaymentStatus newStatus,
        string? reason = null)
    {
        return new TransactionStatusHistory
        {
            Id = TransactionStatusHistoryId.Create(),
            TransactionId = transactionId,
            OldStatus = oldStatus,
            NewStatus = newStatus,
            Reason = reason,
            ChangedAt = DateTime.UtcNow
        };
    }
}
