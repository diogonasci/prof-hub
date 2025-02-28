using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.GroupClass.Events;
public record TeacherAssignedToSuggestionEvent(string SuggestionId, string TeacherId) : IDomainEvent;

