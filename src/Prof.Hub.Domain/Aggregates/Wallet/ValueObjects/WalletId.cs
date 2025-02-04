namespace Prof.Hub.Domain.Aggregates.Wallet.ValueObjects;
public sealed record WalletId(string Value)
{
    public static WalletId Create() => new(Guid.NewGuid().ToString());
}
