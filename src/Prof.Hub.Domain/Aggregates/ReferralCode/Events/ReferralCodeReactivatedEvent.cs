using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.ReferralCode.Events;
public record ReferralCodeReactivatedEvent(string CodeId) : IDomainEvent;
