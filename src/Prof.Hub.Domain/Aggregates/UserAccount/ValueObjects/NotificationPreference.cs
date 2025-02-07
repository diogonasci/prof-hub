using Prof.Hub.Domain.Enums;

namespace Prof.Hub.Domain.Aggregates.UserAccount.ValueObjects;
public record NotificationPreference(
    NotificationType Type,
    bool IsEnabled);
