using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Teacher.Events;
public record TeacherSpecialtyAddedEvent(
    string TeacherId,
    string SubjectArea,
    string Description,
    bool IsVerified
) : IDomainEvent;
