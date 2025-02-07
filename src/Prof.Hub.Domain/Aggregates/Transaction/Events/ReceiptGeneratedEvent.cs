using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Transaction.Events;
public record ReceiptGeneratedEvent(string TransactionId, string ReceiptId) : IDomainEvent;

