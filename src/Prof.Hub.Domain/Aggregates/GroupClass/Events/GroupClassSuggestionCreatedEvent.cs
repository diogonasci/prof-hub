using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.GroupClass.Events;
public record GroupClassSuggestionCreatedEvent(string SuggestionId, string SuggestedBy) : IDomainEvent;
