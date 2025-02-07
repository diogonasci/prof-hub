using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Student.Events;
public record EnrollmentCompletedEvent(string StudentId, string ClassId, decimal Rating) : IDomainEvent;
