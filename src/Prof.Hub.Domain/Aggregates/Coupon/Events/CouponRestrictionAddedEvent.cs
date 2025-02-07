using Prof.Hub.Domain.Aggregates.Coupon.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Coupon.Events;
public record CouponRestrictionAddedEvent(CouponId CouponId, CouponRestrictionType RestrictionType) : IDomainEvent;

