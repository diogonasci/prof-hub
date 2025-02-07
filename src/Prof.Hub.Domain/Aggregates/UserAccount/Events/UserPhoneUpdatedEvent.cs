using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.UserAccount.Events;

public record UserPhoneUpdatedEvent(string AccountId, string? OldPhone, string? NewPhone) : IDomainEvent; 
