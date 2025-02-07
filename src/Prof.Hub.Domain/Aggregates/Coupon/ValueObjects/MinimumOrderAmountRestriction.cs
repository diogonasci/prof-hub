using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Coupon.ValueObjects;
public record MinimumOrderAmountRestriction : CouponRestriction
{
    public Money MinimumAmount { get; }

    public MinimumOrderAmountRestriction(Money minimumAmount)
        : base(CouponRestrictionType.MinimumOrderAmount)
    {
        MinimumAmount = minimumAmount;
    }

    public override Result Validate(StudentId studentId, Money orderAmount)
    {
        if (orderAmount < MinimumAmount)
            return Result.Invalid(new ValidationError($"Valor mínimo de {MinimumAmount.Amount} {MinimumAmount.Currency} requerido"));

        return Result.Success();
    }
}
