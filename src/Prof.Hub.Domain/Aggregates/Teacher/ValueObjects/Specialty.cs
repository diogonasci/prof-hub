using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
public sealed record Specialty
{
    public SubjectArea Area { get; }
    public string Description { get; }
    public bool IsVerified { get; private set; }

    private Specialty(SubjectArea area, string description, bool isVerified = false)
    {
        Area = area;
        Description = description;
        IsVerified = isVerified;
    }

    public static Result<Specialty> Create(SubjectArea area, string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Result.Invalid(new ValidationError("Descrição da especialidade é obrigatória"));

        if (description.Length > 200)
            return Result.Invalid(new ValidationError("Descrição não pode exceder 200 caracteres"));

        return new Specialty(area, description);
    }

    public Specialty Verify() => this with { IsVerified = true };
}
