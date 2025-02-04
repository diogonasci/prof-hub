using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Common.ValueObjects;
public sealed record Rating
{
    public int Value { get; }

    private Rating(int value)
    {
        Value = value;
    }

    public static Result<Rating> Create(int value)
    {
        if (value < 1 || value > 5)
            return Result.Invalid(new ValidationError("A avaliação deve ser um número entre 1 e 5."));

        return new Rating(value);
    }
}
