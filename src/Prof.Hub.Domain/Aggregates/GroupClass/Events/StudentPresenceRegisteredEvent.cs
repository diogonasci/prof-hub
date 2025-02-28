using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.GroupClass.Events;
public record StudentPresenceRegisteredEvent(string ClassId, string StudentId, DateTime PresenceTime) : IDomainEvent;

