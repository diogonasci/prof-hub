using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Student.Events;
public record EnrollmentCanceledEvent(string StudentId, string ClassId) : IDomainEvent;
