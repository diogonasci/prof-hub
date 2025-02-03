namespace Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
public sealed record TeacherId(string Value)
{
    public static TeacherId Create() => new(Guid.NewGuid().ToString());
}
