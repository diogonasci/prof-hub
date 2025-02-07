using Prof.Hub.Domain.Aggregates.GroupClass.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.GroupClass.Events;
public record GroupClassSuggestionRejectedEvent(GroupClassSuggestionId SuggestionId, string Reason) : IDomainEvent;

