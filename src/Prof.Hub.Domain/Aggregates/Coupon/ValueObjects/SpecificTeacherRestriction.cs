using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Coupon.ValueObjects;
public record SpecificTeacherRestriction : CouponRestriction
{
    public TeacherId TeacherId { get; }

    public SpecificTeacherRestriction(TeacherId teacherId)
        : base(CouponRestrictionType.SpecificTeacher)
    {
        TeacherId = teacherId;
    }

    public override Result Validate(StudentId studentId, Money orderAmount)
    {
        // Implementação requer acesso ao contexto da aula
        return Result.Success();
    }
}
