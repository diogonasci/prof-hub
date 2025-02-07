using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Teacher.Events;
public record TeacherStatusChangedEvent(string TeacherId, TeacherStatus NewStatus) : IDomainEvent;

