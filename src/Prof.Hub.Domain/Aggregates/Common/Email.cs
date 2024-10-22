using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Common;
public sealed record Email(string Value)
{
    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !IsValidEmail(value))
            return Result.Invalid(new ValidationError("E-mail inválido."));

        return Result.Success(new Email(value));
    }

    private static bool IsValidEmail(string email)
    {
        return email.Contains("@");
    }
}
