using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Coupon.ValueObjects;
public record MaximumOrderAmountRestriction : CouponRestriction
{
    public Money MaximumAmount { get; }

    public MaximumOrderAmountRestriction(Money maximumAmount)
        : base(CouponRestrictionType.MaximumOrderAmount)
    {
        MaximumAmount = maximumAmount;
    }

    public override Result Validate(StudentId studentId, Money orderAmount)
    {
        if (orderAmount > MaximumAmount)
            return Result.Invalid(new ValidationError($"Valor máximo de {MaximumAmount.Amount} {MaximumAmount.Currency} excedido"));

        return Result.Success();
    }
}
