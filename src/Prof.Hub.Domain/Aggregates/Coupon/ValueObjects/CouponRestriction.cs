using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Coupon.ValueObjects;
public abstract record CouponRestriction
{
    public CouponRestrictionType Type { get; }

    protected CouponRestriction(CouponRestrictionType type)
    {
        Type = type;
    }

    public abstract Result Validate(StudentId studentId, Money orderAmount);
}
