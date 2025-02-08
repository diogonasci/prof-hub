using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.GroupClass.Events;
public record ClassRequirementAddedEvent(
    string ClassId,
    string Description,
    bool IsMandatory) : IDomainEvent;
