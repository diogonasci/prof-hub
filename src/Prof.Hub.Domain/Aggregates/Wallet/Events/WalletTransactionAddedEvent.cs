﻿using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Wallet.Events;
public record WalletTransactionAddedEvent(
    string WalletId,
    string TransactionId,
    decimal PreviousBalance,
    decimal NewBalance,
    decimal TransactionAmount,
    string TransactionType
) : IDomainEvent;
