using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.PrivateClass.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.PrivateClass.Events;
public record PrivateClassRescheduledEvent(
    PrivateClassId ClassId,
    StudentId StudentId,
    ClassSchedule OldSchedule,
    ClassSchedule NewSchedule) : IDomainEvent;
