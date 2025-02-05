using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Student.Events;
public record TeacherAddedToFavoritesEvent(StudentId StudentId, TeacherId TeacherId) : IDomainEvent;

