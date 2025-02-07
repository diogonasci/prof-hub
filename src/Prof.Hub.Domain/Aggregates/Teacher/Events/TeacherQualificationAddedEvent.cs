using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Teacher.Events;
public record TeacherQualificationAddedEvent(string TeacherId, Qualification Qualification) : IDomainEvent;

