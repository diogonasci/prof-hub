using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Transaction.Events;
public record TransactionDetailAddedEvent(string TransactionId, string DetailId) : IDomainEvent;
