using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;

namespace Prof.Hub.Domain.Aggregates.Coupon.ValueObjects;
public record CouponUsage(
    CouponId CouponId,
    StudentId StudentId,
    Money OrderAmount,
    DateTime UsedAt);
