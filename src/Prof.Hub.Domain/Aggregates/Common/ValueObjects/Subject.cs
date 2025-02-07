using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Common.ValueObjects;
public sealed record Subject
{
    public string Name { get; }
    public string Description { get; }
    public SubjectArea Area { get; }
    public EducationLevel Level { get; }
    public string[] Topics { get; }

    private Subject(string name, string description, SubjectArea area, EducationLevel level, string[] topics)
    {
        Name = name;
        Description = description;
        Area = area;
        Level = level;
        Topics = topics;
    }

    public static Result<Subject> Create(
        string name,
        string description,
        SubjectArea area,
        EducationLevel level,
        string[] topics)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(name))
            errors.Add(new ValidationError("Nome é obrigatório."));

        if (string.IsNullOrWhiteSpace(description))
            errors.Add(new ValidationError("Descrição é obrigatória."));

        if (topics == null || !topics.Any())
            errors.Add(new ValidationError("Pelo menos um tópico é obrigatório."));

        if (topics?.Any(t => string.IsNullOrWhiteSpace(t)) == true)
            errors.Add(new ValidationError("Tópicos não podem estar vazios."));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        return new Subject(
            name.Trim(),
            description.Trim(),
            area,
            level,
            topics.Select(t => t.Trim()).ToArray()
        );
    }

    public bool IsCompatibleWith(Grade grade)
    {
        return Level switch
        {
            EducationLevel.All => true,
            EducationLevel.ElementarySchool => grade is >= Grade.FirstYearElementary and <= Grade.FifthYearElementary,
            EducationLevel.MiddleSchool => grade is >= Grade.SixthYearElementary and <= Grade.NinthYearElementary,
            EducationLevel.HighSchool => grade is >= Grade.FirstYearHigh and <= Grade.ThirdYearHigh,
            _ => false
        };
    }

    public bool ContainsTopic(string topic)
    {
        return Topics.Any(t => t.Equals(topic.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}
