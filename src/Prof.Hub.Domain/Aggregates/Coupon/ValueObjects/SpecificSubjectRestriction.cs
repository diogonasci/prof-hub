using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Coupon.ValueObjects;
public record SpecificSubjectRestriction : CouponRestriction
{
    public SubjectArea Subject { get; }

    public SpecificSubjectRestriction(SubjectArea subject)
        : base(CouponRestrictionType.SpecificSubject)
    {
        Subject = subject;
    }

    public override Result Validate(StudentId studentId, Money orderAmount)
    {
        // Implementação requer acesso ao contexto da aula
        return Result.Success();
    }
}
