using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.UserAccount.Events;

public record LoginProviderRemovedEvent(string AccountId, string ProviderType) : IDomainEvent; 
