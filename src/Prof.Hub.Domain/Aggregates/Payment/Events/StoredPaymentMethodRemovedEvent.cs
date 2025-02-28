using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Payment.Events;
public record StoredPaymentMethodRemovedEvent(string PaymentId, string StudentId, string MethodId) : IDomainEvent;

