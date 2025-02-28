using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.GroupClass.Events;
public record StudentPromotedFromWaitingListEvent(string ClassId, string StudentId) : IDomainEvent;

