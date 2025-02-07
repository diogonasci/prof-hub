using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.UserAccount.Events;

public record NotificationPreferencesUpdatedEvent(UserAccountId AccountId, NotificationType Type, bool Enabled) : IDomainEvent; 
