using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Transaction.Events;
public record WalletBalanceUpdatedEvent(string WalletId, decimal NewBalance) : IDomainEvent;
