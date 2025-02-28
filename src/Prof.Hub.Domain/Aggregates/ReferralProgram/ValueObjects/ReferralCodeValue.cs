using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.ReferralProgram.ValueObjects;
public sealed record ReferralCodeValue
{
    public string Value { get; }

    private ReferralCodeValue(string value) => Value = value;

    public static Result<ReferralCodeValue> Create(string value)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(value))
            errors.Add(new ValidationError("O código não pode ser vazio."));

        if (value.Length != 8)
            errors.Add(new ValidationError("O código deve ter 8 caracteres."));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        return new ReferralCodeValue(value);
    }
}
