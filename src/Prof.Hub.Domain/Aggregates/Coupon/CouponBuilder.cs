using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Coupon.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Coupon;

public class CouponBuilder
{
    private string _code;
    private CouponType _type;
    private decimal _percentageDiscount;
    private Money _fixedAmount;
    private DateTime _validFrom;
    private DateTime _validUntil;
    private int? _maxUsesPerStudent;
    private int? _totalMaxUses;
    private CouponRestriction _restriction;

    private CouponBuilder() { }

    public static CouponBuilder Create(string code)
    {
        return new CouponBuilder
        {
            _code = code
        };
    }

    public CouponBuilder AsPercentageDiscount(decimal percentageOff)
    {
        _type = CouponType.PercentageDiscount;
        _percentageDiscount = percentageOff;
        return this;
    }

    public CouponBuilder AsFixedDiscount(Money amount)
    {
        _type = CouponType.FixedDiscount;
        _fixedAmount = amount;
        return this;
    }

    public CouponBuilder AsGiftCard(Money amount)
    {
        _type = CouponType.GiftCard;
        _fixedAmount = amount;
        return this;
    }

    public CouponBuilder ValidBetween(DateTime from, DateTime until)
    {
        _validFrom = from;
        _validUntil = until;
        return this;
    }

    public CouponBuilder WithMaxUsesPerStudent(int maxUses)
    {
        _maxUsesPerStudent = maxUses;
        return this;
    }

    public CouponBuilder WithTotalMaxUses(int maxUses)
    {
        _totalMaxUses = maxUses;
        return this;
    }

    public CouponBuilder WithRestriction(CouponRestriction restriction)
    {
        _restriction = restriction;
        return this;
    }

    public Result<Coupon> Build()
    {
        var errors = new List<ValidationError>();

        // Validações comuns
        if (string.IsNullOrWhiteSpace(_code))
            errors.Add(new ValidationError("Código é obrigatório"));

        ValidateDates(_validFrom, _validUntil, errors);

        // Validações específicas por tipo
        switch (_type)
        {
            case CouponType.PercentageDiscount:
                if (_percentageDiscount <= 0 || _percentageDiscount > 100)
                    errors.Add(new ValidationError("Percentual de desconto deve estar entre 0 e 100"));
                break;

            case CouponType.FixedDiscount:
            case CouponType.GiftCard:
                if (_fixedAmount == null || _fixedAmount.Amount <= 0)
                    errors.Add(new ValidationError("Valor do desconto deve ser maior que zero"));
                break;

            default:
                errors.Add(new ValidationError("Tipo de cupom deve ser especificado"));
                break;
        }

        if (errors.Count > 0)
            return Result.Invalid(errors);

        // Criar o cupom com base no tipo
        var coupon = new Coupon
        {
            Id = CouponId.Create(),
            Code = _code.ToUpper(),
            Type = _type,
            ValidFrom = _validFrom,
            ValidUntil = _validUntil,
            MaxUsesPerStudent = _maxUsesPerStudent,
            TotalMaxUses = _totalMaxUses
        };

        // Definir o valor do desconto
        switch (_type)
        {
            case CouponType.PercentageDiscount:
                var percentageResult = DiscountValue.CreatePercentage(_percentageDiscount);
                if (!percentageResult.IsSuccess)
                    return Result.Invalid(percentageResult.ValidationErrors);
                coupon.Value = percentageResult.Value;
                break;

            case CouponType.FixedDiscount:
            case CouponType.GiftCard:
                var fixedResult = DiscountValue.CreateFixed(_fixedAmount);
                if (!fixedResult.IsSuccess)
                    return Result.Invalid(fixedResult.ValidationErrors);
                coupon.Value = fixedResult.Value;
                break;
        }

        // Adicionar restrição, se houver
        if (_restriction != null)
        {
            var addRestrictionResult = coupon.AddRestriction(_restriction);
            if (!addRestrictionResult.IsSuccess)
                return Result.Invalid(addRestrictionResult.ValidationErrors);
        }

        coupon.RegisterCreatedEvent();

        return coupon;
    }

    private static void ValidateDates(DateTime validFrom, DateTime validUntil, List<ValidationError> errors)
    {
        if (validFrom >= validUntil)
            errors.Add(new ValidationError("Data inicial deve ser anterior à data final"));

        if (validUntil <= DateTime.UtcNow)
            errors.Add(new ValidationError("Data final deve ser futura"));
    }
}
