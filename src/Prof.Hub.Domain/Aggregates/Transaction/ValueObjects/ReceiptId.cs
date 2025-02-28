namespace Prof.Hub.Domain.Aggregates.Transaction.ValueObjects;
public record ReceiptId(string Value)
{
    public static ReceiptId Create() => new(Guid.NewGuid().ToString());
}
