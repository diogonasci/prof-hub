using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.GroupClass.Events;
public record StudentAddedToWaitingListEvent(string ClassId, string StudentId) : IDomainEvent;

