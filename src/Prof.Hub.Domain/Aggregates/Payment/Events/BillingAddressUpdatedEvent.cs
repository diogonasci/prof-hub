using Prof.Hub.Domain.Aggregates.Payment.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Payment.Events;
public record BillingAddressUpdatedEvent(PaymentId PaymentId, StudentId StudentId, BillingAddressId AddressId) : IDomainEvent;

