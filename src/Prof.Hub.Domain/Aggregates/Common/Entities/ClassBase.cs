using Prof.Hub.Domain.Aggregates.Common.Events;
using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Common.Entities;
public abstract class ClassBase : AuditableEntity
{
    private readonly List<ClassMaterial.ClassMaterial> _materials = [];
    private readonly List<ClassFeedback.ClassFeedback> _feedbacks = [];
    private readonly List<StatusChange> _statusHistory = [];
    private readonly List<ClassIssue> _issues = [];
    private readonly List<Attendance> _attendance = [];

    protected const int MIN_DURATION_HOURS = 1;
    protected const int MAX_DURATION_HOURS = 4;
    protected const int MIN_HOURS_BEFORE_START = 2;

    public TeacherId TeacherId { get; protected set; }
    public Subject Subject { get; protected set; }
    public ClassSchedule Schedule { get; protected set; }
    public Price Price { get; protected set; }
    protected ClassStatus Status { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public TimeSpan? EffectiveDuration { get; private set; }

    public IReadOnlyList<ClassMaterial.ClassMaterial> Materials => _materials.AsReadOnly();
    public IReadOnlyList<ClassFeedback.ClassFeedback> Feedbacks => _feedbacks.AsReadOnly();
    public IReadOnlyList<StatusChange> StatusHistory => _statusHistory.AsReadOnly();
    public IReadOnlyList<ClassIssue> Issues => _issues.AsReadOnly();
    public IReadOnlyList<Attendance> Attendance => _attendance.AsReadOnly();

    public abstract string GetId();

    protected ClassBase()
    {
    }

    public ClassStatus GetStatus() => Status;

    public virtual Result Start(IDateTimeProvider dateTimeProvider)
    {
        var canStartResult = CanStart(dateTimeProvider);
        if (!canStartResult.IsSuccess)
            return canStartResult;

        var previousStatus = Status;
        Status = ClassStatus.InProgress;
        StartedAt = dateTimeProvider.UtcNow;

        RecordStatusChange(previousStatus, Status, dateTimeProvider.UtcNow);
        AddDomainEvent(new ClassStartedEvent(GetId()));

        return Result.Success();
    }

    public virtual Result Complete(ClassFeedback.ClassFeedback feedback, IDateTimeProvider dateTimeProvider)
    {
        var canCompleteResult = CanComplete(dateTimeProvider);
        if (!canCompleteResult.IsSuccess)
            return canCompleteResult;

        var previousStatus = Status;
        Status = ClassStatus.Completed;
        CompletedAt = dateTimeProvider.UtcNow;
        EffectiveDuration = CompletedAt - StartedAt;

        _feedbacks.Add(feedback);
        RecordStatusChange(previousStatus, Status, dateTimeProvider.UtcNow);
        AddDomainEvent(new ClassCompletedEvent(GetId(), feedback.OverallRating.Value, feedback.TeachingRating.Value, feedback.MaterialsRating.Value,
            feedback.TeachingRating.Value, feedback.TeacherComment, feedback.TechnicalComment, feedback.IsAnonymous, feedback.HadTechnicalIssues,
            CompletedAt.Value));

        return Result.Success();
    }

    public virtual Result Cancel(IDateTimeProvider dateTimeProvider)
    {
        var canCancelResult = CanCancel(dateTimeProvider);
        if (!canCancelResult.IsSuccess)
            return canCancelResult;

        var previousStatus = Status;
        Status = ClassStatus.Cancelled;

        RecordStatusChange(previousStatus, Status, dateTimeProvider.UtcNow);
        AddDomainEvent(new ClassCanceledEvent(GetId()));

        return Result.Success();
    }

    public Result RegisterAttendance(string participantId, ParticipantType type, DateTime joinTime)
    {
        if (Status != ClassStatus.InProgress)
            return Result.Invalid(new ValidationError("Presença só pode ser registrada em aulas em andamento."));

        var attendance = new Attendance(participantId, type, joinTime);
        _attendance.Add(attendance);

        AddDomainEvent(new AttendanceRegisteredEvent(GetId(), participantId, type.ToString(), joinTime));

        return Result.Success();
    }

    public Result AddMaterial(ClassMaterial.ClassMaterial material, bool isAvailableBeforeClass)
    {
        if (material == null)
            return Result.Invalid(new ValidationError("Material não pode ser nulo."));

        if (_materials.Any(m => m.Id == material.Id))
            return Result.Invalid(new ValidationError("Material já existe."));

        material.SetAvailability(isAvailableBeforeClass);
        _materials.Add(material);
        AddDomainEvent(new MaterialAddedEvent(GetId(), material.Id.Value));

        return Result.Success();
    }

    public Result ReportIssue(string description, ClassIssueType type, IDateTimeProvider dateTimeProvider)
    {
        if (Status != ClassStatus.InProgress)
            return Result.Invalid(new ValidationError("Problemas só podem ser reportados durante a aula."));

        var issue = new ClassIssue(description, type, dateTimeProvider.UtcNow);
        _issues.Add(issue);

        AddDomainEvent(new ClassIssueReportedEvent(GetId(), issue.Description, issue.Type.ToString(), issue.ReportedAt));

        return Result.Success();
    }

    public virtual Result CanStart(IDateTimeProvider dateTimeProvider)
    {
        if (Status != ClassStatus.Scheduled)
            return Result.Invalid(new ValidationError("Somente aulas agendadas podem ser iniciadas."));

        if (Schedule.StartDate <= dateTimeProvider.UtcNow)
            return Result.Invalid(new ValidationError("Aula já passou do horário de início."));

        return Result.Success();
    }

    protected virtual Result CanComplete(IDateTimeProvider dateTimeProvider)
    {
        if (Status != ClassStatus.InProgress)
            return Result.Invalid(new ValidationError("Somente aulas em andamento podem ser completadas."));

        if (!Schedule.HasEnded())
            return Result.Invalid(new ValidationError("Aula ainda não chegou ao horário de término."));

        return Result.Success();
    }

    protected virtual Result CanCancel(IDateTimeProvider dateTimeProvider)
    {
        if (Status != ClassStatus.Scheduled)
            return Result.Invalid(new ValidationError("Somente aulas agendadas podem ser canceladas."));

        if (Schedule.StartDate <= dateTimeProvider.UtcNow.AddHours(MIN_HOURS_BEFORE_START))
            return Result.Invalid(new ValidationError($"Cancelamento deve ser feito com no mínimo {MIN_HOURS_BEFORE_START} horas de antecedência."));

        return Result.Success();
    }

    private void RecordStatusChange(ClassStatus previousStatus, ClassStatus newStatus, DateTime changedAt)
    {
        _statusHistory.Add(new StatusChange(previousStatus, newStatus, changedAt));
    }
}
