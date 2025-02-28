using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.PrivateClass.Events;
public record PrivateClassCanceledEvent(string ClassId, string StudentId) : IDomainEvent;

