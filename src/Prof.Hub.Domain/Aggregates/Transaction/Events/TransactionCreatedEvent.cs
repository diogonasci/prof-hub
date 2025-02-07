using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Transaction.Events;
public record TransactionCreatedEvent(string WalletId, string TransactionId, decimal Amount) : IDomainEvent;

