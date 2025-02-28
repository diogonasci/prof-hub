namespace Prof.Hub.Domain.Aggregates.GroupClass.ValueObjects;
public sealed record GroupClassId(string Value)
{
    public static GroupClassId Create() => new(Guid.NewGuid().ToString());
}
