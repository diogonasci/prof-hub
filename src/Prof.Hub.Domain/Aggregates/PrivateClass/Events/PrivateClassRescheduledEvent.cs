using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.PrivateClass.Events;
public record PrivateClassRescheduledEvent(
    string ClassId,
    string StudentId,
    ClassSchedule OldSchedule,
    ClassSchedule NewSchedule) : IDomainEvent;
