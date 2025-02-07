using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Payment.Events;
public record PaymentRefundedEvent(string PaymentId) : IDomainEvent;

