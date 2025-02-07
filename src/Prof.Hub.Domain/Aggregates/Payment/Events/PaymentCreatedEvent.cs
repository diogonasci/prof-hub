using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Payment.Events;
public record PaymentCreatedEvent(string PaymentId, string StudentId) : IDomainEvent;

