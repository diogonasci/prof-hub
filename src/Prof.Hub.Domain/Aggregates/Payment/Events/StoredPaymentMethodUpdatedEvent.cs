using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Payment.Events;
public record StoredPaymentMethodUpdatedEvent(string PaymentId, string StudentId, string MethodId) : IDomainEvent;

