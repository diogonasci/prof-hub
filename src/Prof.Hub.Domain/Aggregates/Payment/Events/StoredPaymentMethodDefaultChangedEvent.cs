using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Payment.Events;
public record StoredPaymentMethodDefaultChangedEvent(string PaymentId, string StudentId, string MethodId) : IDomainEvent;

