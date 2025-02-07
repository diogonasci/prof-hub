using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.UserAccount.Events;

public record UserPhoneVerifiedEvent(UserAccountId AccountId) : IDomainEvent; 
