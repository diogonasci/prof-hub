using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.UserAccount.Events;

public record UserAccountDeactivatedEvent(UserAccountId AccountId, string Reason) : IDomainEvent; 
