using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Common;
public sealed record Name(string Value)
{
    public static Result<Name> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Invalid(new ValidationError("O nome não pode ser vazio."));

        return Result.Success(new Name(value));
    }
}
