namespace Prof.Hub.Domain.Aggregates.Transaction.ValueObjects;
public record TransactionDetailId(string Value)
{
    public static TransactionDetailId Create() => new(Guid.NewGuid().ToString());
}
