using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Common.ValueObjects;
public sealed record PhoneNumber(string Value)
{
    public static Result<PhoneNumber> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Invalid(new ValidationError("O número de telefone não pode ser vazio."));

        return Result.Success(new PhoneNumber(value));
    }
}
