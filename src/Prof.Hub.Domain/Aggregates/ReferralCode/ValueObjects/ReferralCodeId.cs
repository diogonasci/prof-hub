namespace Prof.Hub.Domain.Aggregates.ReferralCode.ValueObjects;
public sealed record ReferralCodeId(string Value)
{
    public static ReferralCodeId  Create() => new(Guid.NewGuid().ToString());
}
