using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Coupon.ValueObjects;
public record FirstPurchaseRestriction : CouponRestriction
{
    public FirstPurchaseRestriction()
        : base(CouponRestrictionType.FirstPurchase)
    {
    }

    public override Result Validate(StudentId studentId, Money orderAmount)
    {
        // Implementação requer acesso ao histórico de compras do estudante
        return Result.Success();
    }
}
