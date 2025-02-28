using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Common.Events;
public record ClassStartedEvent(string ClassId) : IDomainEvent;
