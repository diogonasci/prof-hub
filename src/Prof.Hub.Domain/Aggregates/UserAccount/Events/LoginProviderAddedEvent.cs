using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.UserAccount.Events;

public record LoginProviderAddedEvent(UserAccountId AccountId, LoginProviderType ProviderType) : IDomainEvent; 
