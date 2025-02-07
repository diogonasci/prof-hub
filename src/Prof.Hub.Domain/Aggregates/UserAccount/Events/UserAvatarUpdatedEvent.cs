using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.UserAccount.Events;

public record UserAvatarUpdatedEvent(UserAccountId AccountId, Uri? OldAvatarUrl, Uri? NewAvatarUrl) : IDomainEvent; 
