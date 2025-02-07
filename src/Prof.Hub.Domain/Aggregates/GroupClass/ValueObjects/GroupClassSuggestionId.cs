namespace Prof.Hub.Domain.Aggregates.GroupClass.ValueObjects;
public record GroupClassSuggestionId(string Value)
{
    public static GroupClassSuggestionId Create() => new(Guid.NewGuid().ToString());
}
