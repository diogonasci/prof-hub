using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.UserAccount.Events;

public record UserEmailUpdatedEvent(string AccountId, string OldEmail, string NewEmail) : IDomainEvent; 
