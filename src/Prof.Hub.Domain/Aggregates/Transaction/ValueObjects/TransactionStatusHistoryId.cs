namespace Prof.Hub.Domain.Aggregates.Transaction.ValueObjects;
public record TransactionStatusHistoryId(string Value)
{
    public static TransactionStatusHistoryId Create() => new(Guid.NewGuid().ToString());
}
