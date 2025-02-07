using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.GroupClass.Events;
public record StudentEnrollmentCanceledEvent(string ClassId, string StudentId) : IDomainEvent;

