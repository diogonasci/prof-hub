using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Coupon.Events;
using Prof.Hub.Domain.Aggregates.Coupon.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Coupon;
public class Coupon : AuditableEntity, IAggregateRoot
{
    private readonly List<CouponUsage> _usageHistory = [];
    private readonly List<CouponRestriction> _restrictions = [];

    public CouponId Id { get; private set; }
    public string Code { get; private set; }
    public CouponType Type { get; private set; }
    public DiscountValue Value { get; private set; }
    public DateTime ValidFrom { get; private set; }
    public DateTime ValidUntil { get; private set; }
    public int? MaxUsesPerStudent { get; private set; }
    public int? TotalMaxUses { get; private set; }
    public CodeStatus Status { get; private set; }
    public bool IsActive { get; private set; }

    public IReadOnlyList<CouponUsage> UsageHistory => _usageHistory.AsReadOnly();
    public IReadOnlyList<CouponRestriction> Restrictions => _restrictions.AsReadOnly();

    private Coupon()
    {
        IsActive = true;
        Status = CodeStatus.Active;
    }

    public static Result<Coupon> CreatePercentageDiscount(
        string code,
        decimal percentageOff,
        DateTime validFrom,
        DateTime validUntil,
        int? maxUsesPerStudent = null,
        int? totalMaxUses = null)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(code))
            errors.Add(new ValidationError("Código é obrigatório"));

        if (percentageOff <= 0 || percentageOff > 100)
            errors.Add(new ValidationError("Percentual de desconto deve estar entre 0 e 100"));

        ValidateDates(validFrom, validUntil, errors);

        if (errors.Count > 0)
            return Result.Invalid(errors);

        var valueResult = DiscountValue.CreatePercentage(percentageOff);
        if (!valueResult.IsSuccess)
            return Result.Invalid(valueResult.ValidationErrors);

        var coupon = new Coupon
        {
            Id = CouponId.Create(),
            Code = code.ToUpper(),
            Type = CouponType.PercentageDiscount,
            Value = valueResult.Value,
            ValidFrom = validFrom,
            ValidUntil = validUntil,
            MaxUsesPerStudent = maxUsesPerStudent,
            TotalMaxUses = totalMaxUses
        };

        coupon.AddDomainEvent(new CouponCreatedEvent(coupon.Id.Value, coupon.Code));
        return coupon;
    }

    public static Result<Coupon> CreateFixedDiscount(
        string code,
        Money fixedAmount,
        DateTime validFrom,
        DateTime validUntil,
        int? maxUsesPerStudent = null,
        int? totalMaxUses = null)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(code))
            errors.Add(new ValidationError("Código é obrigatório"));

        ValidateDates(validFrom, validUntil, errors);

        if (errors.Count > 0)
            return Result.Invalid(errors);

        var valueResult = DiscountValue.CreateFixed(fixedAmount);
        if (!valueResult.IsSuccess)
            return Result.Invalid(valueResult.ValidationErrors);

        var coupon = new Coupon
        {
            Id = CouponId.Create(),
            Code = code.ToUpper(),
            Type = CouponType.FixedDiscount,
            Value = valueResult.Value,
            ValidFrom = validFrom,
            ValidUntil = validUntil,
            MaxUsesPerStudent = maxUsesPerStudent,
            TotalMaxUses = totalMaxUses
        };

        coupon.AddDomainEvent(new CouponCreatedEvent(coupon.Id.Value, coupon.Code));
        return coupon;
    }

    public static Result<Coupon> CreateGiftCard(
        string code,
        Money amount,
        DateTime validFrom,
        DateTime validUntil)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(code))
            errors.Add(new ValidationError("Código é obrigatório"));

        ValidateDates(validFrom, validUntil, errors);

        if (errors.Count > 0)
            return Result.Invalid(errors);

        var valueResult = DiscountValue.CreateFixed(amount);
        if (!valueResult.IsSuccess)
            return Result.Invalid(valueResult.ValidationErrors);

        var coupon = new Coupon
        {
            Id = CouponId.Create(),
            Code = code.ToUpper(),
            Type = CouponType.GiftCard,
            Value = valueResult.Value,
            ValidFrom = validFrom,
            ValidUntil = validUntil,
            MaxUsesPerStudent = 1,
            TotalMaxUses = 1
        };

        coupon.AddDomainEvent(new CouponCreatedEvent(coupon.Id.Value, coupon.Code));
        return coupon;
    }

    public Result AddRestriction(CouponRestriction restriction)
    {
        if (_restrictions.Any(r => r.Type == restriction.Type))
            return Result.Invalid(new ValidationError("Restrição já existe"));

        _restrictions.Add(restriction);
        AddDomainEvent(new CouponRestrictionAddedEvent(Id.Value, restriction.Type.ToString()));

        return Result.Success();
    }

    public Result Use(StudentId studentId, Money orderAmount)
    {
        var validationResult = ValidateUsage(studentId, orderAmount);
        if (!validationResult.IsSuccess)
            return validationResult;

        var usage = new CouponUsage(Id, studentId, orderAmount, DateTime.UtcNow);
        _usageHistory.Add(usage);

        if (IsFullyUsed())
        {
            Status = CodeStatus.FullyUsed;
            AddDomainEvent(new CouponFullyUsedEvent(Id.Value));
        }

        AddDomainEvent(new CouponUsedEvent(Id.Value, studentId.Value));
        return Result.Success();
    }

    public Result<Money> CalculateDiscount(Money orderAmount)
    {
        return Value.CalculateDiscount(orderAmount);
    }

    public void Deactivate()
    {
        if (!IsActive) return;

        IsActive = false;
        Status = CodeStatus.Inactive;
        AddDomainEvent(new CouponDeactivatedEvent(Id.Value));
    }

    private Result ValidateUsage(StudentId studentId, Money orderAmount)
    {
        var errors = new List<ValidationError>();

        if (!IsActive)
            errors.Add(new ValidationError("Cupom está inativo"));

        if (Status != CodeStatus.Active)
            errors.Add(new ValidationError("Cupom não está ativo"));

        if (DateTime.UtcNow < ValidFrom)
            errors.Add(new ValidationError("Cupom ainda não está válido"));

        if (DateTime.UtcNow > ValidUntil)
        {
            Status = CodeStatus.Expired;
            errors.Add(new ValidationError("Cupom expirado"));
        }

        var studentUsages = _usageHistory.Count(u => u.StudentId == studentId);
        if (MaxUsesPerStudent.HasValue && studentUsages >= MaxUsesPerStudent.Value)
            errors.Add(new ValidationError("Limite de uso por estudante atingido"));

        if (TotalMaxUses.HasValue && _usageHistory.Count >= TotalMaxUses.Value)
        {
            Status = CodeStatus.FullyUsed;
            errors.Add(new ValidationError("Limite total de usos atingido"));
        }

        foreach (var restriction in _restrictions)
        {
            var restrictionResult = restriction.Validate(studentId, orderAmount);
            if (!restrictionResult.IsSuccess)
                errors.AddRange(restrictionResult.ValidationErrors);
        }

        if (errors.Count > 0)
            return Result.Invalid(errors);

        return Result.Success();
    }

    private bool IsFullyUsed()
    {
        if (!TotalMaxUses.HasValue) return false;
        return _usageHistory.Count >= TotalMaxUses.Value;
    }

    private static void ValidateDates(DateTime validFrom, DateTime validUntil, List<ValidationError> errors)
    {
        if (validFrom >= validUntil)
            errors.Add(new ValidationError("Data inicial deve ser anterior à data final"));

        if (validUntil <= DateTime.UtcNow)
            errors.Add(new ValidationError("Data final deve ser futura"));
    }
}
