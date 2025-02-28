namespace Prof.Hub.Domain.Aggregates.PrivateClass.ValueObjects;
public sealed record PrivateClassId(string Value)
{
    public static PrivateClassId Create() => new(Guid.NewGuid().ToString());
}
