using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Payment.Events;
public record PaymentProcessingEvent(string PaymentId) : IDomainEvent;

