using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.GroupClass.Events;
public record GroupClassSharedEvent(
    string ClassId,
    string SharedBy,
    string NetworkName) : IDomainEvent;

