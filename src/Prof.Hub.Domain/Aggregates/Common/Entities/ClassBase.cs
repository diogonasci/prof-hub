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

    public TeacherId TeacherId { get; protected set; }
    public Subject Subject { get; protected set; }
    public ClassSchedule Schedule { get; protected set; }
    public Price Price { get; protected set; }
    public ClassStatus Status { get; protected set; }

    public abstract string GetId();

    public IReadOnlyList<ClassMaterial.ClassMaterial> Materials => _materials.AsReadOnly();
    public IReadOnlyList<ClassFeedback.ClassFeedback> Feedbacks => _feedbacks.AsReadOnly();

    protected ClassBase()
    {
    }

    public virtual Result Start()
    {
        if (Status != ClassStatus.Scheduled)
            return Result.Invalid(new ValidationError("Somente aulas agendadas podem ser iniciadas."));

        Status = ClassStatus.InProgress;
        AddDomainEvent(new ClassStartedEvent(GetId()));

        return Result.Success();
    }

    public virtual Result Complete(ClassFeedback.ClassFeedback feedback)
    {
        if (Status != ClassStatus.InProgress)
            return Result.Invalid(new ValidationError("Somente aulas em andamento podem ser completadas."));

        Status = ClassStatus.Completed;
        _feedbacks.Add(feedback);
        AddDomainEvent(new ClassCompletedEvent(GetId(), feedback));

        return Result.Success();
    }

    public virtual Result Cancel()
    {
        if (Status != ClassStatus.Scheduled)
            return Result.Invalid(new ValidationError("Somente aulas agendadas podem ser canceladas."));

        Status = ClassStatus.Cancelled;
        AddDomainEvent(new ClassCanceledEvent(GetId()));

        return Result.Success();
    }

    protected void AddMaterial(ClassMaterial.ClassMaterial material)
    {
        _materials.Add(material);
        AddDomainEvent(new MaterialAddedEvent(GetId(), material.Id));
    }

    protected void AddFeedback(ClassFeedback.ClassFeedback feedback)
    {
        _feedbacks.Add(feedback);
        AddDomainEvent(new FeedbackAddedEvent(GetId(), feedback.Id));
    }
}
