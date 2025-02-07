using Prof.Hub.Domain.Aggregates.Transaction.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Transaction.Events;
public record ReceiptGeneratedEvent(TransactionId TransactionId, ReceiptId ReceiptId) : IDomainEvent;

