using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.GroupClass.Events;
public record ParticipantLimitUpdatedEvent(string ClassId, int NewLimit) : IDomainEvent;

