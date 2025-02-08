using Prof.Hub.Domain.Aggregates.GroupClass.Events;
using Prof.Hub.Domain.Aggregates.GroupClass.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.GroupClass.Entities;
public class GroupClassSuggestion : AuditableEntity
{
    private readonly List<StudentInterest> _interestedStudents = [];

    public GroupClassSuggestionId Id { get; private set; }
    public StudentId SuggestedBy { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public SubjectArea SubjectArea { get; private set; }
    public EducationLevel EducationLevel { get; private set; }
    public SuggestionStatus Status { get; private set; }
    public TeacherId? AssignedTeacher { get; private set; }
    public string? RejectionReason { get; private set; }
    public int MinimumStudents { get; private set; }
    public TimeSpan PreferredDuration { get; private set; }

    public IReadOnlyList<StudentInterest> InterestedStudents => _interestedStudents.AsReadOnly();

    private GroupClassSuggestion() { }

    public static Result<GroupClassSuggestion> Create(
        StudentId suggestedBy,
        string title,
        string description,
        SubjectArea subjectArea,
        EducationLevel educationLevel,
        int minimumStudents,
        TimeSpan preferredDuration)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(title))
            errors.Add(new ValidationError("Título é obrigatório"));

        if (string.IsNullOrWhiteSpace(description))
            errors.Add(new ValidationError("Descrição é obrigatória"));

        if (minimumStudents < 3)
            errors.Add(new ValidationError("Mínimo de 3 alunos necessário"));

        if (preferredDuration < TimeSpan.FromHours(1) || preferredDuration > TimeSpan.FromHours(4))
            errors.Add(new ValidationError("Duração deve ser entre 1 e 4 horas"));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        var suggestion = new GroupClassSuggestion
        {
            Id = GroupClassSuggestionId.Create(),
            SuggestedBy = suggestedBy,
            Title = title,
            Description = description,
            SubjectArea = subjectArea,
            EducationLevel = educationLevel,
            Status = SuggestionStatus.Pending,
            MinimumStudents = minimumStudents,
            PreferredDuration = preferredDuration
        };

        suggestion.AddDomainEvent(new GroupClassSuggestionCreatedEvent(suggestion.Id.Value, suggestedBy.Value, suggestion.Title, suggestion.Description));
        return suggestion;
    }

    public Result AddInterestedStudent(StudentId studentId)
    {
        if (Status != SuggestionStatus.Pending)
            return Result.Invalid(new ValidationError("Sugestão não está mais aberta para interessados"));

        if (_interestedStudents.Any(i => i.StudentId == studentId))
            return Result.Invalid(new ValidationError("Estudante já demonstrou interesse"));

        var interest = new StudentInterest(studentId, DateTime.UtcNow);
        _interestedStudents.Add(interest);

        AddDomainEvent(new StudentInterestedInSuggestionEvent(Id.Value, studentId));

        if (_interestedStudents.Count >= MinimumStudents)
            AddDomainEvent(new SuggestionReachedMinimumStudentsEvent(Id.Value, ));

        return Result.Success();
    }

    public Result AssignTeacher(TeacherId teacherId)
    {
        if (Status != SuggestionStatus.Pending)
            return Result.Invalid(new ValidationError("Sugestão não está pendente"));

        AssignedTeacher = teacherId;
        Status = SuggestionStatus.Assigned;

        AddDomainEvent(new TeacherAssignedToSuggestionEvent(Id.Value, teacherId.Value));
        return Result.Success();
    }

    public Result Approve()
    {
        if (Status != SuggestionStatus.Assigned)
            return Result.Invalid(new ValidationError("Professor precisa ser atribuído primeiro"));

        Status = SuggestionStatus.Approved;
        AddDomainEvent(new GroupClassSuggestionApprovedEvent(Id.Value));

        return Result.Success();
    }

    public Result Reject(string reason)
    {
        if (Status != SuggestionStatus.Pending && Status != SuggestionStatus.Assigned)
            return Result.Invalid(new ValidationError("Sugestão não pode ser rejeitada"));

        Status = SuggestionStatus.Rejected;
        RejectionReason = reason;

        AddDomainEvent(new GroupClassSuggestionRejectedEvent(Id.Value, reason));

        return Result.Success();
    }
}
