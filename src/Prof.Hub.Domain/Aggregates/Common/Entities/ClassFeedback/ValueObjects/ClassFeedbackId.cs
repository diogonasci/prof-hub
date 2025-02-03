namespace Prof.Hub.Domain.Aggregates.Common.Entities.ClassFeedback.ValueObjects;
public sealed record ClassFeedbackId(string Value)
{
    public static ClassFeedbackId Create() => new(Guid.NewGuid().ToString());
}
