using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.ReferralCode.Events;
public record ReferralCodeExpirationRenewedEvent(string CodeId, DateTime OldExpirationDate, DateTime NewExpirationDate) : IDomainEvent;
