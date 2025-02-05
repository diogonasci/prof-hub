using Prof.Hub.Domain.Aggregates.ReferralCode.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.ReferralProgram.Events;
public record InviteAcceptedEvent(ReferralInviteId InviteId, StudentId StudentId) : IDomainEvent;

