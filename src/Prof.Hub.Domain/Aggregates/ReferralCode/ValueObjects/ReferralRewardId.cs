namespace Prof.Hub.Domain.Aggregates.ReferralCode.ValueObjects;
public sealed record ReferralRewardId(string Value)
{
    public static ReferralRewardId Create() => new(Guid.NewGuid().ToString());
}
