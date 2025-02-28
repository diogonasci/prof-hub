namespace Prof.Hub.Domain.Aggregates.PrivateClass.ValueObjects;
public record ScheduleChange
{
    public PrivateClassId ClassId { get; }
    public DateTime OldStartDate { get; }
    public TimeSpan OldDuration { get; }
    public DateTime NewStartDate { get; }
    public TimeSpan NewDuration { get; }
    public DateTime ChangedAt { get; }

    private ScheduleChange(
        PrivateClassId classId,
        DateTime oldStartDate,
        TimeSpan oldDuration,
        DateTime newStartDate,
        TimeSpan newDuration,
        DateTime changedAt)
    {
        ClassId = classId;
        OldStartDate = oldStartDate;
        OldDuration = oldDuration;
        NewStartDate = newStartDate;
        NewDuration = newDuration;
        ChangedAt = changedAt;
    }

    public static ScheduleChange Create(
        PrivateClassId classId,
        DateTime oldStartDate,
        TimeSpan oldDuration,
        DateTime newStartDate,
        TimeSpan newDuration,
        DateTime changedAt)
    {
        return new ScheduleChange(
            classId,
            oldStartDate,
            oldDuration,
            newStartDate,
            newDuration,
            changedAt);
    }
}

