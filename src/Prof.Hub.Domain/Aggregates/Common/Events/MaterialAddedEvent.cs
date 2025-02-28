using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Common.Events;
public record MaterialAddedEvent(string ClassId, string MaterialId) : IDomainEvent;
