namespace Prof.Hub.Domain.Aggregates.UserAccount.ValueObjects;
public sealed record UserAccountId(string Value)
{
    public static UserAccountId Create() => new(Guid.NewGuid().ToString());
}
