using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Common.ValueObjects;
public record RewardAmount
{
    public decimal Value { get; }

    private RewardAmount(decimal value) => Value = value;

    public static Result<RewardAmount> Create(decimal value)
    {
        if (value < 0)
            return Result.Invalid(new ValidationError("Valor não pode ser negativo."));

        return new RewardAmount(value);
    }
}
