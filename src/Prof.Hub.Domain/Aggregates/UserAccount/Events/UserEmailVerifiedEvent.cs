using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.UserAccount.Events;

public record UserEmailVerifiedEvent(string AccountId) : IDomainEvent; 
