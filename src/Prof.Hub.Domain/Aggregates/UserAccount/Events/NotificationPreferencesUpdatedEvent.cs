using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.UserAccount.Events;

public record NotificationPreferencesUpdatedEvent(string AccountId, NotificationType Type, bool Enabled) : IDomainEvent; 
