namespace Prof.Hub.Domain.Aggregates.Payment.ValueObjects;
public sealed record PaymentId(string Value)
{
    public static PaymentId Create() => new(Guid.NewGuid().ToString());
}
