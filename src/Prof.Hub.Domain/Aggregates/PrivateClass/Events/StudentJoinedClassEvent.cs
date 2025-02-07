using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.PrivateClass.Events;
public record StudentJoinedClassEvent(
    string ClassId,
    string StudentId,
    DateTime JoinTime) : IDomainEvent;
