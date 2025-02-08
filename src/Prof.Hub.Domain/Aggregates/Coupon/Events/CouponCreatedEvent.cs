using Prof.Hub.Domain.Aggregates.Coupon.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Coupon.Events;
public record CouponCreatedEvent(string CouponId, string Code) : IDomainEvent;

