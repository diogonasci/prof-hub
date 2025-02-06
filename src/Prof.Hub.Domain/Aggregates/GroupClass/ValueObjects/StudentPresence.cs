using Prof.Hub.Domain.Aggregates.Student.ValueObjects;

namespace Prof.Hub.Domain.Aggregates.GroupClass.ValueObjects;
public sealed record StudentPresence(StudentId StudentId, DateTime PresenceTime);
