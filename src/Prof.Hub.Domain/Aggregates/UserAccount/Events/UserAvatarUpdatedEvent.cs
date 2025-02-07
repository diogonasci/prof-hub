using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.UserAccount.Events;

public record UserAvatarUpdatedEvent(string AccountId, Uri? OldAvatarUrl, Uri? NewAvatarUrl) : IDomainEvent; 
