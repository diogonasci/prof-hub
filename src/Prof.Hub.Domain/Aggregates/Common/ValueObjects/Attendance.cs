using Prof.Hub.Domain.Enums;

namespace Prof.Hub.Domain.Aggregates.Common.ValueObjects;
public record Attendance(string ParticipantId, ParticipantType Type, DateTime JoinTime);
