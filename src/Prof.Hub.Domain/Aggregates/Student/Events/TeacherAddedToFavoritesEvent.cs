using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Student.Events;
public record TeacherAddedToFavoritesEvent(string StudentId, string TeacherId) : IDomainEvent;

