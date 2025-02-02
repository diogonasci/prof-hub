using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Common.ValueObjects;
public sealed record ClassSchedule
{
    public DateTime StartDate { get; }
    public TimeSpan Duration { get; }
    public DateTime EndDate => StartDate.Add(Duration);

    private ClassSchedule(DateTime startDate, TimeSpan duration)
    {
        StartDate = startDate;
        Duration = duration;
    }

    public static Result<ClassSchedule> Create(DateTime startDate, TimeSpan duration)
    {
        if (startDate < DateTime.UtcNow)
            return Result.Invalid(new ValidationError("Data de início deve estar no futuro."));

        if (duration < TimeSpan.FromMinutes(60) || duration > TimeSpan.FromHours(4))
            return Result.Invalid(new ValidationError("A duração da aula não pode ser inferior a 1 hora ou superior a 4 horas."));

        return Result.Success(new ClassSchedule(startDate, duration));
    }
}
