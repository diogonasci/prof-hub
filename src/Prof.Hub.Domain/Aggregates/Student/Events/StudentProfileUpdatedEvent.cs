using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Student.Events;
public record StudentProfileUpdatedEvent(StudentId StudentId, StudentProfile Profile) : IDomainEvent;

