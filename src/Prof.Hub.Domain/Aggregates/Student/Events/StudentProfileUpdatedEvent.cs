using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Student.Events;
public record StudentProfileUpdatedEvent(
    string StudentId,
    string Name,
    string Email,
    string PhoneNumber,
    Grade? Grade,
    string? AvatarUrl,
    string ReferralCode
) : IDomainEvent;

