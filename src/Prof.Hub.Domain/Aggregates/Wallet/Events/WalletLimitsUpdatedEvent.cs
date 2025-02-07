using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Wallet.Events;
public record WalletLimitsUpdatedEvent(string WalletId, decimal DailyLimit, decimal MonthlyLimit) : IDomainEvent;
