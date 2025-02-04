namespace Prof.Hub.Domain.Aggregates.ReferralCode.ValueObjects;
public sealed record ReferralInviteId(string Value)
{
    public static ReferralInviteId Create() => new(Guid.NewGuid().ToString());
}
