using Prof.Hub.Domain.Aggregates.Common.Entities;
using Prof.Hub.Domain.Aggregates.Common.Entities.ClassFeedback;
using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.PrivateClass.Events;
using Prof.Hub.Domain.Aggregates.PrivateClass.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.PrivateClass;

public class PrivateClass : ClassBase, IAggregateRoot
{
    private readonly IDateTimeProvider _dateTimeProvider;

    private readonly List<ClassNote> _notes = [];
    private readonly List<ScheduleChange> _scheduleChanges = [];

    private const int MIN_HOURS_BEFORE_START = 2;
    private const int MAX_DURATION_HOURS = 4;
    private const int MIN_DURATION_HOURS = 1;
    private const int MAX_EXTENSION_MINUTES = 30;

    public PrivateClassId Id { get; private set; }
    public StudentId StudentId { get; private set; }
    public Uri MeetingUrl { get; private set; }
    public bool StudentPresent { get; private set; }
    public bool TeacherPresent { get; private set; }
    public DateTime? StudentJoinedAt { get; private set; }
    public DateTime? TeacherJoinedAt { get; private set; }

    public IReadOnlyList<ClassNote> Notes => _notes.AsReadOnly();
    public IReadOnlyList<ScheduleChange> ScheduleChanges => _scheduleChanges.AsReadOnly();

    private PrivateClass(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public override string GetId() => Id.Value;

    public static Result<PrivateClass> Create(
        TeacherId teacherId,
        StudentId studentId,
        Subject subject,
        DateTime startDate,
        TimeSpan duration,
        Price price,
        Uri meetingUrl,
        IDateTimeProvider dateTimeProvider)
    {
        var errors = new List<ValidationError>();

        if (teacherId.Value == studentId.Value)
            errors.Add(new ValidationError("Professor e aluno não podem ser a mesma pessoa."));

        if (startDate < dateTimeProvider.UtcNow.AddHours(MIN_HOURS_BEFORE_START))
            errors.Add(new ValidationError($"Aula deve ser agendada com no mínimo {MIN_HOURS_BEFORE_START} horas de antecedência."));

        if (duration.TotalHours < MIN_DURATION_HOURS || duration.TotalHours > MAX_DURATION_HOURS)
            errors.Add(new ValidationError($"Duração deve ser entre {MIN_DURATION_HOURS} e {MAX_DURATION_HOURS} horas."));

        if (!IsValidMeetingUrl(meetingUrl))
            errors.Add(new ValidationError("URL da reunião inválida."));

        var classScheduleResult = ClassSchedule.Create(startDate, duration);
        if (!classScheduleResult.IsSuccess)
            errors.AddRange(classScheduleResult.ValidationErrors);

        if (errors.Count > 0)
            return Result.Invalid(errors);

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

    public Result Reschedule(DateTime newStartDate, IDateTimeProvider dateTimeProvider, TimeSpan? newDuration = null)
    {
        if (Status != ClassStatus.Scheduled)
            return Result.Invalid(new ValidationError("Apenas aulas agendadas podem ser remarcadas."));

        if (Schedule.StartDate <= dateTimeProvider.UtcNow.AddHours(MIN_HOURS_BEFORE_START))
            return Result.Invalid(new ValidationError("Não é possível remarcar com menos de 2 horas do início."));

        var duration = newDuration ?? Schedule.Duration;
        var scheduleResult = ClassSchedule.Create(newStartDate, duration);

        if (!scheduleResult.IsSuccess)
            return Result.Invalid(scheduleResult.ValidationErrors);

        var oldSchedule = Schedule;
        Schedule = scheduleResult.Value;

        var change = ScheduleChange.Create(
            Id,
            oldSchedule.StartDate,
            oldSchedule.Duration,
            newStartDate,
            duration,
            dateTimeProvider.UtcNow);

        _scheduleChanges.Add(change);

        AddDomainEvent(new PrivateClassRescheduledEvent(
            Id.Value,
            StudentId.Value,
            oldSchedule.StartDate,
            oldSchedule.Duration,
            Schedule.StartDate,
            Schedule.Duration
        ));

        return Result.Success();
    }

    public Result ExtendDuration(TimeSpan additionalTime, IDateTimeProvider dateTimeProvider)
    {
        if (Status != ClassStatus.InProgress)
            return Result.Invalid(new ValidationError("Apenas aulas em andamento podem ser estendidas."));

        if (additionalTime.TotalMinutes > MAX_EXTENSION_MINUTES)
            return Result.Invalid(new ValidationError($"Extensão máxima permitida é de {MAX_EXTENSION_MINUTES} minutos."));

        var newDuration = Schedule.Duration.Add(additionalTime);
        if (newDuration.TotalHours > MAX_DURATION_HOURS)
            return Result.Invalid(new ValidationError($"Duração total não pode exceder {MAX_DURATION_HOURS} horas."));

        var oldSchedule = Schedule;
        var scheduleResult = ClassSchedule.Create(Schedule.StartDate, newDuration);

        if (!scheduleResult.IsSuccess)
            return Result.Invalid(scheduleResult.ValidationErrors);

        Schedule = scheduleResult.Value;

        var change = ScheduleChange.Create(
            Id,
            oldSchedule.StartDate,
            oldSchedule.Duration,
            Schedule.StartDate,
            Schedule.Duration,
            dateTimeProvider.UtcNow);

        _scheduleChanges.Add(change);

        AddDomainEvent(new PrivateClassExtendedEvent(Id.Value, StudentId.Value, additionalTime));

        return Result.Success();
    }

    public Result RegisterStudentPresence(DateTime joinTime)
    {
        if (Status != ClassStatus.InProgress)
            return Result.Invalid(new ValidationError("Presença só pode ser registrada em aulas em andamento."));

        StudentPresent = true;
        StudentJoinedAt = joinTime;

        AddDomainEvent(new StudentJoinedClassEvent(Id.Value, StudentId.Value, joinTime));

        return Result.Success();
    }

    public Result RegisterTeacherPresence(DateTime joinTime)
    {
        if (Status != ClassStatus.InProgress)
            return Result.Invalid(new ValidationError("Presença só pode ser registrada em aulas em andamento."));

        TeacherPresent = true;
        TeacherJoinedAt = joinTime;

        AddDomainEvent(new TeacherJoinedClassEvent(Id.Value, TeacherId.Value, joinTime));

        return Result.Success();
    }

    public Result AddNote(string content, IDateTimeProvider dateTimeProvider)
    {
        if (Status == ClassStatus.Draft || Status == ClassStatus.Cancelled)
            return Result.Invalid(new ValidationError("Não é possível adicionar notas neste estado."));

        var noteResult = ClassNote.Create(content, dateTimeProvider.UtcNow);
        if (!noteResult.IsSuccess)
            return Result.Invalid(noteResult.ValidationErrors);

        _notes.Add(noteResult.Value);

        AddDomainEvent(new ClassNoteAddedEvent(Id.Value, noteResult.Value.Content));

        return Result.Success();
    }

    public override Result Start()
    {
        var result = base.Start();
        if (!result.IsSuccess)
            return result;

        AddDomainEvent(new PrivateClassStartedEvent(Id.Value, StudentId.Value));
        return Result.Success();
    }

    public override Result CanComplete(IDateTimeProvider dateTimeProvider)
    {
        var baseResult = base.CanComplete(dateTimeProvider);
        if (!baseResult.IsSuccess)
            return baseResult;

        if (!HasBothParticipantsPresent())
            return Result.Invalid(new ValidationError("Professor e aluno devem estar presentes para completar a aula."));

        return Result.Success();
    }

    public override Result Complete(ClassFeedback feedback)
    {
        if (!StudentPresent || !TeacherPresent)
            return Result.Invalid(new ValidationError("Não é possível completar aula sem a presença do professor e aluno."));

        var result = base.Complete(feedback);
        if (!result.IsSuccess)
            return result;

        AddDomainEvent(new PrivateClassCompletedEvent(
            Id.Value,
            StudentId.Value,
            feedback.OverallRating.Value,
            feedback.TeachingRating.Value,
            feedback.MaterialsRating.Value,
            feedback.TechnicalRating.Value,
            feedback.TeacherComment,
            feedback.TechnicalComment,
            feedback.IsAnonymous,
            feedback.HadTechnicalIssues
        ));

        return Result.Success();
    }

    public override Result Cancel()
    {
        var result = base.Cancel();
        if (!result.IsSuccess)
            return result;

        AddDomainEvent(new PrivateClassCanceledEvent(Id.Value, StudentId.Value));

        return Result.Success();
    }

    private static bool IsValidMeetingUrl(Uri url)
    {
        if (url == null) return false;

        return url.Scheme == Uri.UriSchemeHttps &&
               (url.Host.Contains("meet.") ||
                url.Host.Contains("zoom.") ||
                url.Host.Contains("teams."));
    }
}
