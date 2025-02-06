using Prof.Hub.Domain.Aggregates.ReferralCode.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.ReferralCode.Events;
public record ReferralCodeReactivatedEvent(ReferralCodeId CodeId) : IDomainEvent;
