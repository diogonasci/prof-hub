using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.GroupClass.Events;
public record SuggestionReachedMinimumStudentsEvent(
    string SuggestionId,
    int CurrentStudentCount) : IDomainEvent;
