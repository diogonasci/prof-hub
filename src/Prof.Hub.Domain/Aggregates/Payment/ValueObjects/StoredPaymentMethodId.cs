namespace Prof.Hub.Domain.Aggregates.Payment.ValueObjects;
public record StoredPaymentMethodId(string Value)
{
    public static StoredPaymentMethodId Create() => new(Guid.NewGuid().ToString());
}
