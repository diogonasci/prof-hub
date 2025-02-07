using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.GroupClass.Events;
public record ThematicClassPublishedEvent(string ClassId) : IDomainEvent;
