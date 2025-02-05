using Prof.Hub.Domain.Aggregates.Common.Entities;
using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.GroupClass.Events;
using Prof.Hub.Domain.Aggregates.GroupClass.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.GroupClass;

public class GroupClass : ClassBase, IAggregateRoot
{
    public GroupClassId Id { get; private set; }
    public string Title { get; private set; }
    public string Slug { get; private set; }
    public Uri ThumbnailUrl { get; private set; }
    public string Description { get; private set; }
    public ParticipantLimit ParticipantLimit { get; private set; }
    public HashSet<StudentId> Participants { get; private set; } = [];

    private GroupClass()
    {
    }

    public override string GetId() => Id.Value;

    public static Result<GroupClass> Create(
        string title,
        string slug,
        TeacherId teacherId,
        Subject subject,
        ClassSchedule schedule,
        Price price,
        Uri thumbnailUrl,
        string description,
        ParticipantLimit participantLimit)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(title))
            errors.Add(new ValidationError("Título é obrigatório."));

        if (string.IsNullOrWhiteSpace(description))
            errors.Add(new ValidationError("Descrição é obrigatória."));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        var groupClass = new GroupClass
        {
            Id = GroupClassId.Create(),
            Title = title,
            Slug = slug,
            TeacherId = teacherId,
            Subject = subject,
            Schedule = schedule,
            Price = price,
            ThumbnailUrl = thumbnailUrl,
            Description = description,
            ParticipantLimit = participantLimit,
            Status = ClassStatus.Draft
        };

        return groupClass;
    }

    public Result EnrollStudent(StudentId studentId)
    {
        var errors = new List<ValidationError>();

        if (Status != ClassStatus.Published)
            errors.Add(new ValidationError("A aula não está aberta para inscrições."));

        if (Participants.Count >= ParticipantLimit.Value)
            errors.Add(new ValidationError("As vagas para está aula esgotaram."));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        Participants.Add(studentId);
        AddDomainEvent(new StudentEnrolledEvent(Id, studentId));

        return Result.Success();
    }

    public Result Publish()
    {
        if (Status != ClassStatus.Draft)
            return Result.Invalid(new ValidationError("Somente aulas em rascunho podem ser publicadas."));

        Status = ClassStatus.Published;
        AddDomainEvent(new ThematicClassPublishedEvent(Id));

        return Result.Success();
    }
}
