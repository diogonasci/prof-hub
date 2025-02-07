using Prof.Hub.Domain.Aggregates.Transaction.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Transaction.Events;
public record TransactionDetailAddedEvent(TransactionId TransactionId, TransactionDetailId DetailId) : IDomainEvent;
