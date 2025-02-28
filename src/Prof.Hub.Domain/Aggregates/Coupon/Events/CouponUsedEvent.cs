using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Coupon.Events;
public record CouponUsedEvent(string CouponId, string StudentId) : IDomainEvent;

