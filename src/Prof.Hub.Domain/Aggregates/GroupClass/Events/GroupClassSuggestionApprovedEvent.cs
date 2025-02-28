using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.GroupClass.Events;
public record GroupClassSuggestionApprovedEvent(
    string SuggestionId,
    string TeacherId) : IDomainEvent;
