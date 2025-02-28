using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Teacher.Events;
public record TeacherCreatedEvent(string TeacherId) : IDomainEvent;

