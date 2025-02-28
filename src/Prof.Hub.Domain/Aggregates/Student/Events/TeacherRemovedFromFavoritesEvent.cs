using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Student.Events;
public record TeacherRemovedFromFavoritesEvent(string StudentId, string TeacherId) : IDomainEvent;

