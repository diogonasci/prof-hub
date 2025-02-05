using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Transaction.Events;
public record BonusAddedEvent(string WalletId, decimal BonusAmount, string Reason) : IDomainEvent;

