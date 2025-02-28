namespace Prof.Hub.Domain.Aggregates.Transaction.ValueObjects;
public sealed record TransactionId(string Value)
{
    public static TransactionId Create() => new(Guid.NewGuid().ToString());
}
