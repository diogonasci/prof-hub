using Prof.Hub.Domain.Aggregates.PrivateClass.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.PrivateClass.Events;
public record PrivateClassExtendedEvent(
    PrivateClassId ClassId,
    StudentId StudentId,
    TimeSpan AdditionalTime) : IDomainEvent;
