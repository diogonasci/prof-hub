using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Teacher.Events;
public record TeacherProfileUpdatedEvent(TeacherId TeacherId, TeacherProfile Profile) : IDomainEvent;

