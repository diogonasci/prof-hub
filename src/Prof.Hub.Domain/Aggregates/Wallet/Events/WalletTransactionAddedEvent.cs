using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Wallet.Events;
public record WalletTransactionAddedEvent(
    string WalletId,
    string TransactionId,
    decimal PreviousBalance,
    decimal NewBalance,
    decimal TransactionAmount,
    TransactionType TransactionType
) : IDomainEvent;
