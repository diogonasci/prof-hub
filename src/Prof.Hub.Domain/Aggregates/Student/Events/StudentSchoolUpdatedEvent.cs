using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Student.Events;
public record StudentSchoolUpdatedEvent(StudentId StudentId, School School) : IDomainEvent;
