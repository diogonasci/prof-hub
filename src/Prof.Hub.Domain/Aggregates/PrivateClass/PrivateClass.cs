using Prof.Hub.Domain.Aggregates.Common.Entities;
using Prof.Hub.Domain.Aggregates.Common.Entities.ClassFeedback;
using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.PrivateClass.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.PrivateClass;

public class PrivateClass : ClassBase, IAggregateRoot
{
    public PrivateClassId Id { get; private set; }
    public StudentId StudentId { get; private set; }
    public Uri MeetingUrl { get; private set; }

    private PrivateClass()
    {
    }

    public static Result<PrivateClass> Create(
        TeacherId teacherId,
        StudentId studentId,
        Subject subject,
        DateTime startDate,
        TimeSpan duration,
        Price price,
        Uri meetingUrl)
    {
        var classScheduleResult = ClassSchedule.Create(startDate, duration);

        if (!classScheduleResult.IsSuccess)
            return Result.Invalid(classScheduleResult.ValidationErrors);

        if (meetingUrl == null)
            return Result.Invalid(new ValidationError("URL da reunião é obrigatória."));

        var privateClass = new PrivateClass
        {
            Id = PrivateClassId.Create(),
            TeacherId = teacherId,
            StudentId = studentId,
            Subject = subject,
            Schedule = classScheduleResult.Value,
            Price = price,
            MeetingUrl = meetingUrl,
            Status = ClassStatus.Scheduled
        };

        return privateClass;
    }

    public override Result Start()
    {
        var result = base.Start();
        if (!result.IsSuccess)
            return result;

        // Lógica adicional específica para aulas particulares
        AddDomainEvent(new PrivateClassStartedEvent(Id, StudentId));
        return Result.Success();
    }

    public override Result Complete(ClassFeedback feedback)
    {
        var result = base.Complete(feedback);
        if (!result.IsSuccess)
            return result;

        // Lógica adicional específica para aulas particulares
        AddDomainEvent(new PrivateClassCompletedEvent(Id, StudentId, feedback));
        return Result.Success();
    }

    public override Result Cancel()
    {
        var result = base.Cancel();
        if (!result.IsSuccess)
            return result;

        // Lógica adicional específica para aulas particulares
        AddDomainEvent(new PrivateClassCanceledEvent(Id, StudentId));
        return Result.Success();
    }
}
