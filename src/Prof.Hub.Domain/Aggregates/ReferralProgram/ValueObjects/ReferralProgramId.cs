namespace Prof.Hub.Domain.Aggregates.ReferralProgram.ValueObjects;
public sealed record ReferralProgramId(string Value)
{
    public static ReferralProgramId Create() => new(Guid.NewGuid().ToString());
}
