using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Common.Events;
public record AttendanceRegisteredEvent(
    string ClassId,
    string ParticipantId,
    string Type,
    DateTime JoinTime) : IDomainEvent;
