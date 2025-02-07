using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.PrivateClass.Events;
public record PrivateClassExtendedEvent(
    string ClassId,
    string StudentId,
    TimeSpan AdditionalTime) : IDomainEvent;
