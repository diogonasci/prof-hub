using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
public sealed record TimeSlot
{
    public DayOfWeek DayOfWeek { get; }
    public TimeSpan StartTime { get; }
    public TimeSpan EndTime { get; }

    private TimeSlot(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
    {
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
    }

    public static Result<TimeSlot> Create(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
    {
        var errors = new List<ValidationError>();

        if (endTime <= startTime)
            errors.Add(new ValidationError("Início do horário não pode ser superior ao fim."));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        return new TimeSlot(dayOfWeek, startTime, endTime);
    }

    public bool Overlaps(TimeSlot other)
    {
        return DayOfWeek == other.DayOfWeek &&
               StartTime < other.EndTime &&
               other.StartTime < EndTime;
    }
}
