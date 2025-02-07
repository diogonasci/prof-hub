using Prof.Hub.Domain.Aggregates.GroupClass.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.GroupClass.Events;
public record ParticipantLimitUpdatedEvent(string ClassId, ParticipantLimit NewLimit) : IDomainEvent;

