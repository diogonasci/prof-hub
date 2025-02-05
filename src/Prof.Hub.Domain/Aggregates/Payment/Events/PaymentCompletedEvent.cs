using Prof.Hub.Domain.Aggregates.Payment.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Payment.Events;
public record PaymentCompletedEvent(PaymentId PaymentId) : IDomainEvent;

