using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Payment.Events;
public record BillingAddressAddedEvent(string PaymentId, string StudentId, string AddressId) : IDomainEvent;

