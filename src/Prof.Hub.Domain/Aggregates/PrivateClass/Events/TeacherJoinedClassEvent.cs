using Prof.Hub.Domain.Aggregates.PrivateClass.ValueObjects;
using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.PrivateClass.Events;
public record TeacherJoinedClassEvent(
    PrivateClassId ClassId,
    TeacherId TeacherId,
    DateTime JoinTime) : IDomainEvent;
