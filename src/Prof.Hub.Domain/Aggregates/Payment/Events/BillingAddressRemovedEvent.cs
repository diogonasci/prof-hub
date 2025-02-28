using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Payment.Events;
public record BillingAddressRemovedEvent(string PaymentId, string StudentId, string AddressId) : IDomainEvent;

