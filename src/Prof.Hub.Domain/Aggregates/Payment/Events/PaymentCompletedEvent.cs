using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Payment.Events;
public record PaymentCompletedEvent(string PaymentId) : IDomainEvent;

