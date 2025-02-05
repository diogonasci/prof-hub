using Prof.Hub.Domain.Aggregates.GroupClass.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.GroupClass.Events;
public record StudentEnrolledEvent(GroupClassId ClassId, StudentId StudentId) : IDomainEvent;
