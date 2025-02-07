using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.ReferralProgram.Events;
public record InviteSentEvent(string CodeId, string InviteId) : IDomainEvent;

