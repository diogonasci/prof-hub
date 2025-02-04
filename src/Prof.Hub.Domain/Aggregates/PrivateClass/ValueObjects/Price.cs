using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.PrivateClass.ValueObjects;

public sealed record Price(decimal Value)
{
    public static Result<Price> Create(decimal value)
    {
        if (value <= 0)
            return Result.Invalid(new ValidationError("O preço deve ser maior que zero."));

        return new Price(value);
    }
}

