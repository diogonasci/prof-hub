using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.PrivateClass.Events;
public record TeacherJoinedClassEvent(
    string ClassId,
    string TeacherId,
    DateTime JoinTime) : IDomainEvent;
