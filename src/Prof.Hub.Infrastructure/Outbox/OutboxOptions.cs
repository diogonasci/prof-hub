﻿namespace Prof.Hub.Infrastructure.Outbox;
public sealed class OutboxOptions
{
    public int IntervalInSeconds { get; init; }

    public int BatchSize { get; init; }
}