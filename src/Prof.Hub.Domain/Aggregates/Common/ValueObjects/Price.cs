using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Common.ValueObjects;

public sealed record Price
{
    public Money Value { get; }

    private Price(Money value)
    {
        Value = value;
    }

    public static Result<Price> Create(Money value)
    {
        if (value.Amount <= 0)
            return Result.Invalid(new ValidationError("O preço deve ser maior que zero."));

        return new Price(value);
    }

    public static Result<Price> Create(decimal amount, string currency = "BRL")
    {
        var moneyResult = Money.Create(amount, currency);
        if (!moneyResult.IsSuccess)
            return Result.Invalid(moneyResult.ValidationErrors);

        return Create(moneyResult.Value);
    }

    public static implicit operator Money(Price price) => price.Value;
}

