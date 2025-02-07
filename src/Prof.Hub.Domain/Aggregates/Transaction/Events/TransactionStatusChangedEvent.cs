using Prof.Hub.Domain.Aggregates.Transaction.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Transaction.Events;
public record TransactionStatusChangedEvent(TransactionId TransactionId, PaymentStatus OldStatus, PaymentStatus NewStatus) : IDomainEvent;

