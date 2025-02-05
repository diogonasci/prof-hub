using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Student.ValueObjects;
public sealed record School
{
    public string Name { get; }
    public string City { get; }
    public string State { get; }
    public bool IsVerified { get; private set; }

    private School(string name, string city, string state, bool isVerified = false)
    {
        Name = name;
        City = city;
        State = state;
        IsVerified = isVerified;
    }

    public static Result<School> Create(string name, string city, string state)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(name))
            errors.Add(new ValidationError("Nome da escola é obrigatório."));

        if (string.IsNullOrWhiteSpace(city))
            errors.Add(new ValidationError("Cidade é obrigatória."));

        if (string.IsNullOrWhiteSpace(state))
            errors.Add(new ValidationError("Estado é obrigatório."));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        return new School(name, city, state);
    }

    public School Verify() => this with { IsVerified = true };
}
