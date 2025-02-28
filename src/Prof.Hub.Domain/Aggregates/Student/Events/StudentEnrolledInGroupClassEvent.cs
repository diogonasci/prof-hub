using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Student.Events;
public record StudentEnrolledInGroupClassEvent(string StudentId, string GroupClassId) : IDomainEvent;

