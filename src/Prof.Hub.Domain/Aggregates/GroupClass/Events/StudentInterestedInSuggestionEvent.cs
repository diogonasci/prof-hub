using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.GroupClass.Events;
public record StudentInterestedInSuggestionEvent(string SuggestionId, string StudentId) : IDomainEvent;

