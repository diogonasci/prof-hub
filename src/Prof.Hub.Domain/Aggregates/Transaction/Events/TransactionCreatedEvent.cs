using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Transaction.ValueObjects;
using Prof.Hub.Domain.Aggregates.Wallet.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Transaction.Events;
public record TransactionCreatedEvent(WalletId WalletId, TransactionId TransactionId, Money Amount) : IDomainEvent;

