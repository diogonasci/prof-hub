namespace Prof.Hub.Domain.Aggregates.Coupon.ValueObjects;
public sealed record CouponId(string Value)
{
    public static CouponId Create() => new(Guid.NewGuid().ToString());
}
