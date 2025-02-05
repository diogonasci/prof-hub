using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Student.ValueObjects;
public sealed record Balance
{
    public Money Amount { get; }

    private Balance(Money amount)
    {
        Amount = amount;
    }

    public static Result<Balance> Create(decimal amount, string currency = "BRL")
    {
        var moneyResult = Money.Create(amount, currency);

        if (!moneyResult.IsSuccess)
            return Result.Invalid(moneyResult.ValidationErrors);

        return new Balance(moneyResult.Value);
    }

    public Result<Balance> Add(Money amount)
    {
        var result = Amount + amount;
        if (!result.IsSuccess)
            return Result.Invalid(result.ValidationErrors);

        return new Balance(result.Value);
    }

    public Result<Balance> Subtract(Money amount)
    {
        var result = Amount - amount;
        if (!result.IsSuccess)
            return Result.Invalid(result.ValidationErrors);

        return new Balance(result.Value);
    }
}
