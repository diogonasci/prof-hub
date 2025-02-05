using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Teacher.Events;
public record TeacherAvailabilityUpdatedEvent(TeacherId TeacherId) : IDomainEvent;

