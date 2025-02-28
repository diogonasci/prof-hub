using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
public sealed record TeacherAvailability
{
    private readonly List<TimeSlot> _timeSlots = new();
    private const int MIN_WEEKLY_HOURS = 4;
    private const int MAX_DAILY_HOURS = 12;

    public IReadOnlyList<TimeSlot> TimeSlots => _timeSlots.AsReadOnly();

    private TeacherAvailability() { }

    public static TeacherAvailability Create() => new();

    public Result AddTimeSlot(TimeSlot newSlot)
    {
        var errors = new List<ValidationError>();

        if (_timeSlots.Any(slot => slot.Overlaps(newSlot)))
            errors.Add(new ValidationError("Horário conflita com disponibilidade existente"));

        var dailyHours = _timeSlots
            .Where(s => s.DayOfWeek == newSlot.DayOfWeek)
            .Sum(s => s.Duration.TotalHours) + newSlot.Duration.TotalHours;

        if (dailyHours > MAX_DAILY_HOURS)
            errors.Add(new ValidationError($"Não pode exceder {MAX_DAILY_HOURS} horas por dia"));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        _timeSlots.Add(newSlot);
        return Result.Success();
    }

    public Result RemoveTimeSlot(TimeSlot slot)
    {
        var totalWeeklyHours = GetTotalWeeklyHours() - slot.Duration.TotalHours;

        if (totalWeeklyHours < MIN_WEEKLY_HOURS)
            return Result.Invalid(new ValidationError($"Professor deve manter no mínimo {MIN_WEEKLY_HOURS} horas semanais"));

        _timeSlots.Remove(slot);
        return Result.Success();
    }

    private double GetTotalWeeklyHours()
    {
        return _timeSlots.Sum(s => s.Duration.TotalHours);
    }

    public bool HasMinimumAvailability()
    {
        return GetTotalWeeklyHours() >= MIN_WEEKLY_HOURS;
    }
}
