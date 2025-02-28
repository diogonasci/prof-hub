using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Transaction.Events;
public record TransactionStatusChangedEvent(string TransactionId, string OldStatus, string NewStatus) : IDomainEvent;

