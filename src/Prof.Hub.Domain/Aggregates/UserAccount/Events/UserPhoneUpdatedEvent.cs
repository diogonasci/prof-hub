using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.UserAccount.Events;

public record UserPhoneUpdatedEvent(UserAccountId AccountId, PhoneNumber? OldPhone, PhoneNumber? NewPhone) : IDomainEvent; 
