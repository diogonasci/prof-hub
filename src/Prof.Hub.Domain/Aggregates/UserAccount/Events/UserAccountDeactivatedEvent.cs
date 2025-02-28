using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.UserAccount.Events;

public record UserAccountDeactivatedEvent(string AccountId, string Reason) : IDomainEvent; 
