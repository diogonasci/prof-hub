using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Student.Events;
public record TeacherFavoriteNoteUpdatedEvent(string StudentId, string TeacherId, string Note) : IDomainEvent;
