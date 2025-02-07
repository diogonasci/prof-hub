using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.UserAccount.Events;

public record UserEmailUpdatedEvent(UserAccountId AccountId, Email OldEmail, Email NewEmail) : IDomainEvent; 
