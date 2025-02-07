using Prof.Hub.Domain.Aggregates.Coupon.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Coupon.Events;
public record CouponUsedEvent(CouponId CouponId, StudentId StudentId) : IDomainEvent;

