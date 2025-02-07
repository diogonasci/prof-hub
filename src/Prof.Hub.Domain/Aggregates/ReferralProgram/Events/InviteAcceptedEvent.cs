using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.ReferralProgram.Events;
public record InviteAcceptedEvent(string InviteId, string StudentId) : IDomainEvent;

