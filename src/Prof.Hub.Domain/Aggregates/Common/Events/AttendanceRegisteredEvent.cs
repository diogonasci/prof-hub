using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Common.Events;
public record AttendanceRegisteredEvent(
    string ClassId,
    string ParticipantId,
    ParticipantType Type,
    DateTime JoinTime) : IDomainEvent;
