using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.PrivateClass.Events;
public record PrivateClassRescheduledEvent(
    string ClassId,
    string StudentId,
    DateTime OldStartDate,
    TimeSpan OldDuration,
    DateTime NewStartDate,
    TimeSpan NewDuration
) : IDomainEvent;
