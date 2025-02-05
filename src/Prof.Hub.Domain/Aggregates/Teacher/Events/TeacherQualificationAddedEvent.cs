using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Teacher.Events;
public record TeacherQualificationAddedEvent(TeacherId TeacherId, Qualification Qualification) : IDomainEvent;

