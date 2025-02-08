using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Teacher.Events;
public record TeacherRateUpdatedEvent(
    string TeacherId,
    decimal Amount,
    string Currency,
    DateTime EffectiveFrom
) : IDomainEvent;
