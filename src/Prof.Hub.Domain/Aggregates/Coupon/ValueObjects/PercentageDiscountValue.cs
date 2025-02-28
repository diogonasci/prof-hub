using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Coupon.ValueObjects;
public record PercentageDiscountValue : DiscountValue
{
    public decimal Percentage { get; }

    internal PercentageDiscountValue(decimal percentage)
    {
        Percentage = percentage;
    }

    public override Result<Money> CalculateDiscount(Money orderAmount)
    {
        var discountAmount = orderAmount.Amount * (Percentage / 100);
        return Money.Create(discountAmount, orderAmount.Currency);
    }
}
