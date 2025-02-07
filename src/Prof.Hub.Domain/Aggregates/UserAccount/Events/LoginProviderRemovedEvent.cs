using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.UserAccount.Events;

public record LoginProviderRemovedEvent(UserAccountId AccountId, LoginProviderType ProviderType) : IDomainEvent; 
