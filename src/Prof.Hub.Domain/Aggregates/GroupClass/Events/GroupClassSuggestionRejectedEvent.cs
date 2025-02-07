using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.GroupClass.Events;
public record GroupClassSuggestionRejectedEvent(string SuggestionId, string Reason) : IDomainEvent;

