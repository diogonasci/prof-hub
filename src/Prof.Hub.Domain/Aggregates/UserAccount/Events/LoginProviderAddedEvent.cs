using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.UserAccount.Events;

public record LoginProviderAddedEvent(string AccountId, string ProviderType) : IDomainEvent; 
