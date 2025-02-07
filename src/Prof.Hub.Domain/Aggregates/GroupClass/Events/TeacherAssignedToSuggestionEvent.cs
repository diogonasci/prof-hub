using Prof.Hub.Domain.Aggregates.GroupClass.ValueObjects;
using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.GroupClass.Events;
public record TeacherAssignedToSuggestionEvent(GroupClassSuggestionId SuggestionId, TeacherId TeacherId) : IDomainEvent;

