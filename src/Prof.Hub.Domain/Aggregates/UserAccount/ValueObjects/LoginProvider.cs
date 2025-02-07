using Prof.Hub.Domain.Enums;

namespace Prof.Hub.Domain.Aggregates.UserAccount.ValueObjects;
public record LoginProvider(
    LoginProviderType Type,
    string ExternalId,
    string DisplayName,
    DateTime ConnectedAt);
