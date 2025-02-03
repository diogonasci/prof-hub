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
        if (endTime <= startTime)
            return Result<TimeSlot>.Invalid(new List<ValidationError>
                {
                    new("Início do horário não pode ser superior ao fim.")
                });

        return Result<TimeSlot>.Success(new TimeSlot(dayOfWeek, startTime, endTime));
    }

    public bool Overlaps(TimeSlot other)
    {
        return DayOfWeek == other.DayOfWeek &&
               StartTime < other.EndTime &&
               other.StartTime < EndTime;
    }
}
