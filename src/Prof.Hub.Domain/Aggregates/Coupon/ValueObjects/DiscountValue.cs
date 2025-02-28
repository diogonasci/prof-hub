using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Coupon.ValueObjects;
public abstract record DiscountValue
{
    protected DiscountValue() { }

    public static Result<DiscountValue> CreatePercentage(decimal percentage)
    {
        if (percentage <= 0 || percentage > 100)
            return Result.Invalid(new ValidationError("Percentual deve estar entre 0 e 100"));

        return new PercentageDiscountValue(percentage);
    }

    public static Result<DiscountValue> CreateFixed(Money amount)
    {
        if (amount.Amount <= 0)
            return Result.Invalid(new ValidationError("Valor deve ser maior que zero"));

        return new FixedDiscountValue(amount);
    }

    public abstract Result<Money> CalculateDiscount(Money orderAmount);
}

