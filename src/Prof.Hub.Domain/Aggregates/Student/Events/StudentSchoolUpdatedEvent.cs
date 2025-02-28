using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Student.Events;
public record StudentSchoolUpdatedEvent(
    string StudentId,
    string SchoolName,
    string City,
    string State,
    bool IsVerified
) : IDomainEvent;
