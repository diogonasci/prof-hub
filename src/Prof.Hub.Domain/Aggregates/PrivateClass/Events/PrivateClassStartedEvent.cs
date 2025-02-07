using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.PrivateClass.Events;
public record PrivateClassStartedEvent(string ClassId, string StudentId) : IDomainEvent;
