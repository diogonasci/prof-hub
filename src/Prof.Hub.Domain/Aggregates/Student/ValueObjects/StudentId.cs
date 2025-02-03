namespace Prof.Hub.Domain.Aggregates.Student.ValueObjects;
public sealed record StudentId(string Value)
{
    public static StudentId Create() => new(Guid.NewGuid().ToString());
}
