using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.UserAccount.Events;

public record UserPasswordUpdatedEvent(UserAccountId AccountId) : IDomainEvent; 
