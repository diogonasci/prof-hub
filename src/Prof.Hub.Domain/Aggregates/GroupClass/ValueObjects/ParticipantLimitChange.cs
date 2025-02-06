namespace Prof.Hub.Domain.Aggregates.GroupClass.ValueObjects;
public sealed record ParticipantLimitChange(ParticipantLimit Limit, DateTime ChangedAt, string Reason);
