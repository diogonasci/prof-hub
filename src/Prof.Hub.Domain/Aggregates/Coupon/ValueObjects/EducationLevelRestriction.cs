using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Coupon.ValueObjects;
public record EducationLevelRestriction : CouponRestriction
{
    public EducationLevel Level { get; }

    public EducationLevelRestriction(EducationLevel level)
        : base(CouponRestrictionType.EducationLevel)
    {
        Level = level;
    }

    public override Result Validate(StudentId studentId, Money orderAmount)
    {
        // Implementação requer acesso ao perfil do estudante
        return Result.Success();
    }
}
