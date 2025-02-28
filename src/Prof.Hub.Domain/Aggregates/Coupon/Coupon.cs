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

    public CouponId Id { get; internal set; }
    public string Code { get; internal set; }
    public CouponType Type { get; internal set; }
    public DiscountValue Value { get; internal set; }
    public DateTime ValidFrom { get; internal set; }
    public DateTime ValidUntil { get; internal set; }
    public int? MaxUsesPerStudent { get; internal set; }
    public int? TotalMaxUses { get; internal set; }
    public CodeStatus Status { get; private set; }
    public bool IsActive { get; private set; }

    public IReadOnlyList<CouponUsage> UsageHistory => _usageHistory.AsReadOnly();
    public IReadOnlyList<CouponRestriction> Restrictions => _restrictions.AsReadOnly();

    internal Coupon()
    {
        IsActive = true;
        Status = CodeStatus.Active;
    }

    // Método interno para registrar o evento de criação
    internal void RegisterCreatedEvent()
    {
        AddDomainEvent(new CouponCreatedEvent(Id.Value, Code));
    }

    // Método interno para registrar o evento de restrição adicionada
    internal void RegisterRestrictionAddedEvent(CouponRestriction restriction)
    {
        AddDomainEvent(new CouponRestrictionAddedEvent(Id.Value, restriction.Type.ToString()));
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
}
