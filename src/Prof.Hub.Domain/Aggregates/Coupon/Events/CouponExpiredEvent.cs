using Prof.Hub.Domain.Aggregates.Coupon.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Coupon.Events;
public record CouponExpiredEvent(CouponId CouponId) : IDomainEvent;

