using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;

public sealed record Qualification(string Title, string Institution, DateTime ObtainedAt, bool IsVerified = false)
{
    public static Result<Qualification> Create(string title, string institution, DateTime obtainedAt, IDateTimeProvider dateTimeProvider)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(title))
            errors.Add(new ValidationError("Título deve ser informado."));

        if (string.IsNullOrWhiteSpace(institution))
            errors.Add(new ValidationError("Instituição deve ser informada."));

        if (obtainedAt > dateTimeProvider.UtcNow)
            errors.Add(new ValidationError("Data de obtenção não pode estar no futuro."));

        if (errors.Count != 0)
            return Result.Invalid(errors);

        return new Qualification(title, institution, obtainedAt);
    }

    public Qualification Verify() => this with { IsVerified = true };
}
