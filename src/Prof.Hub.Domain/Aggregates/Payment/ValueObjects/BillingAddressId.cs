namespace Prof.Hub.Domain.Aggregates.Payment.ValueObjects;
public record BillingAddressId(string Value)
{
    public static BillingAddressId Create() => new(Guid.NewGuid().ToString());
}
