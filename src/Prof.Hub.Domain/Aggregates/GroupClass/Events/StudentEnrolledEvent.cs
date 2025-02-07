using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.GroupClass.Events;
public record StudentEnrolledEvent(string ClassId, string StudentId) : IDomainEvent;
