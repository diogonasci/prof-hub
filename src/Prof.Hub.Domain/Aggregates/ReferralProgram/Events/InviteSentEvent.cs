using Prof.Hub.Domain.Aggregates.ReferralCode.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.ReferralProgram.Events;
public record InviteSentEvent(ReferralCodeId CodeId, ReferralInviteId InviteId) : IDomainEvent;

