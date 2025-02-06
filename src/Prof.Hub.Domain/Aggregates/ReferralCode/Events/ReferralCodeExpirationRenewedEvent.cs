using Prof.Hub.Domain.Aggregates.ReferralCode.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.ReferralCode.Events;
public record ReferralCodeExpirationRenewedEvent(ReferralCodeId CodeId, DateTime OldExpirationDate, DateTime NewExpirationDate) : IDomainEvent;
