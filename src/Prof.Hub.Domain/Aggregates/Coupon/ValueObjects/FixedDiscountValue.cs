using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Coupon.ValueObjects;
public record FixedDiscountValue : DiscountValue
{
    public Money Amount { get; }

    internal FixedDiscountValue(Money amount)
    {
        Amount = amount;
    }

    public override Result<Money> CalculateDiscount(Money orderAmount)
    {
        if (Amount.Currency != orderAmount.Currency)
            return Result.Invalid(new ValidationError("Moedas diferentes"));

        return Amount.Amount > orderAmount.Amount
            ? Money.Create(orderAmount.Amount, orderAmount.Currency)
            : Money.Create(Amount.Amount, Amount.Currency);
    }
}
