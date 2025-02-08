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
    private readonly HashSet<StudentId> _participants = [];
    private readonly HashSet<StudentId> _waitingList = [];
    private readonly HashSet<StudentPresence> _presenceList = [];
    private readonly List<GroupDiscount> _discounts = [];
    private readonly List<ParticipantLimitChange> _limitChanges = [];
    private readonly List<ClassRequirement> _requirements = [];
    private readonly List<SocialShare> _shares = [];

    private const int MIN_PARTICIPANTS_TO_START = 3;
    private const int MAX_ENROLLMENTS_PER_STUDENT = 3;
    private const int MAX_WAITING_LIST = 10;

    public GroupClassId Id { get; private set; }
    public string Title { get; private set; }
    public string Slug { get; private set; }
    public Uri ThumbnailUrl { get; private set; }
    public string Description { get; private set; }
    public ParticipantLimit ParticipantLimit { get; private set; }
    public bool AllowLateEnrollment { get; private set; }
    public DateTime? EnrollmentDeadline { get; private set; }

    public IReadOnlySet<StudentId> Participants => _participants;
    public IReadOnlySet<StudentId> WaitingList => _waitingList;
    public IReadOnlySet<StudentPresence> PresenceList => _presenceList;
    public IReadOnlyList<GroupDiscount> Discounts => _discounts.AsReadOnly();
    public IReadOnlyList<ParticipantLimitChange> LimitChanges => _limitChanges.AsReadOnly();
    public IReadOnlyList<ClassRequirement> Requirements => _requirements.AsReadOnly();
    public IReadOnlyList<SocialShare> Shares => _shares.AsReadOnly();

    private GroupClass() { }

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
        ParticipantLimit participantLimit,
        bool allowLateEnrollment = false,
        DateTime? enrollmentDeadline = null)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(title))
            errors.Add(new ValidationError("Título é obrigatório."));

        if (string.IsNullOrWhiteSpace(description))
            errors.Add(new ValidationError("Descrição é obrigatória."));

        if (enrollmentDeadline.HasValue && enrollmentDeadline.Value >= schedule.StartDate)
            errors.Add(new ValidationError("Data limite de inscrição deve ser anterior ao início da aula."));

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
            AllowLateEnrollment = allowLateEnrollment,
            EnrollmentDeadline = enrollmentDeadline,
            Status = ClassStatus.Draft
        };

        groupClass._limitChanges.Add(
            new ParticipantLimitChange(participantLimit, DateTime.UtcNow, "Criação da aula"));

        return groupClass;
    }

    public Result EnrollStudent(StudentId studentId, IDateTimeProvider dateTimeProvider)
    {
        var errors = new List<ValidationError>();

        if (Status != ClassStatus.Published)
            errors.Add(new ValidationError("A aula não está aberta para inscrições."));

        if (IsEnrollmentClosed(dateTimeProvider))
            errors.Add(new ValidationError("Inscrições encerradas."));

        if (HasStarted() && !AllowLateEnrollment)
            errors.Add(new ValidationError("Não são permitidas inscrições após o início da aula."));

        if (_participants.Contains(studentId))
            errors.Add(new ValidationError("Estudante já está inscrito."));

        var studentEnrollments = GetStudentEnrollmentCount(studentId);
        if (studentEnrollments >= MAX_ENROLLMENTS_PER_STUDENT)
            errors.Add(new ValidationError($"Limite de {MAX_ENROLLMENTS_PER_STUDENT} inscrições por estudante."));

        if (!MeetsRequirements(studentId))
            errors.Add(new ValidationError("Estudante não atende aos pré-requisitos."));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        if (_participants.Count >= ParticipantLimit.Value)
        {
            if (_waitingList.Count >= MAX_WAITING_LIST)
                return Result.Invalid(new ValidationError("Lista de espera está cheia."));

            _waitingList.Add(studentId);
            AddDomainEvent(new StudentAddedToWaitingListEvent(Id.Value, studentId.Value));
            return Result.Success();
        }

        _participants.Add(studentId);
        AddDomainEvent(new StudentEnrolledEvent(Id.Value, studentId.Value));

        return Result.Success();
    }

    public Result CancelEnrollment(StudentId studentId)
    {
        if (!_participants.Contains(studentId))
            return Result.Invalid(new ValidationError("Estudante não está inscrito."));

        if (HasStarted())
            return Result.Invalid(new ValidationError("Não é possível cancelar após o início da aula."));

        _participants.Remove(studentId);

        // Tenta mover primeiro da lista de espera
        var nextStudent = _waitingList.FirstOrDefault();
        if (nextStudent != null)
        {
            _waitingList.Remove(nextStudent);
            _participants.Add(nextStudent);
            AddDomainEvent(new StudentPromotedFromWaitingListEvent(Id.Value, nextStudent.Value));
        }

        AddDomainEvent(new StudentEnrollmentCanceledEvent(Id.Value, studentId.Value));
        return Result.Success();
    }

    public Result UpdateParticipantLimit(ParticipantLimit newLimit, string reason)
    {
        if (newLimit.Value < _participants.Count)
            return Result.Invalid(new ValidationError("Novo limite não pode ser menor que número atual de participantes."));

        ParticipantLimit = newLimit;
        _limitChanges.Add(new ParticipantLimitChange(newLimit, DateTime.UtcNow, reason));

        // Tenta mover alunos da lista de espera se aumentou o limite
        while (_participants.Count < ParticipantLimit.Value && _waitingList.Any())
        {
            var nextStudent = _waitingList.First();
            _waitingList.Remove(nextStudent);
            _participants.Add(nextStudent);
            AddDomainEvent(new StudentPromotedFromWaitingListEvent(Id.Value, nextStudent.Value));
        }

        AddDomainEvent(new ParticipantLimitUpdatedEvent(Id.Value, newLimit.Value));

        return Result.Success();
    }

    public Result AddGroupDiscount(int minParticipants, decimal percentageDiscount)
    {
        var discountResult = GroupDiscount.Create(minParticipants, percentageDiscount);
        if (!discountResult.IsSuccess)
            return Result.Invalid(discountResult.ValidationErrors);

        _discounts.Add(discountResult.Value);
        AddDomainEvent(new GroupDiscountAddedEvent(Id.Value, minParticipants, percentageDiscount));

        return Result.Success();
    }

    public Result AddRequirement(string description, bool isMandatory)
    {
        var requirement = new ClassRequirement(description, isMandatory);

        _requirements.Add(requirement);
        AddDomainEvent(new ClassRequirementAddedEvent(Id.Value, description, isMandatory));

        return Result.Success();
    }

    public Result RegisterPresence(StudentId studentId, DateTime presenceTime)
    {
        if (!_participants.Contains(studentId))
            return Result.Invalid(new ValidationError("Estudante não está inscrito."));

        if (Status != ClassStatus.InProgress)
            return Result.Invalid(new ValidationError("Aula não está em andamento."));

        var presence = new StudentPresence(studentId, presenceTime);
        _presenceList.Add(presence);

        AddDomainEvent(new StudentPresenceRegisteredEvent(Id.Value, studentId.Value, presenceTime));
        return Result.Success();
    }

    public override Result Start()
    {
        if (_participants.Count < MIN_PARTICIPANTS_TO_START)
            return Result.Invalid(new ValidationError(
                $"Mínimo de {MIN_PARTICIPANTS_TO_START} participantes necessário para iniciar."));

        var result = base.Start();
        if (!result.IsSuccess)
            return result;

        return Result.Success();
    }

    public override Result CanStart(IDateTimeProvider dateTimeProvider)
    {
        var baseResult = base.CanStart(dateTimeProvider);
        if (!baseResult.IsSuccess)
            return baseResult;

        if (_participants.Count < MIN_PARTICIPANTS_TO_START)
            return Result.Invalid(new ValidationError($"Mínimo de {MIN_PARTICIPANTS_TO_START} participantes necessário."));

        return Result.Success();
    }

    public Price GetPriceWithDiscount()
    {
        var applicableDiscount = _discounts
            .Where(d => d.MinParticipants <= _participants.Count)
            .OrderByDescending(d => d.PercentageDiscount)
            .FirstOrDefault();

        if (applicableDiscount == null)
            return Price;

        var discountAmount = Price.Value.Amount * (applicableDiscount.PercentageDiscount / 100m);
        var discountedMoneyResult = Money.Create(Price.Value.Amount - discountAmount, Price.Value.Currency);

        if (!discountedMoneyResult.IsSuccess)
            return Price;

        var discountedPriceResult = Price.Create(discountedMoneyResult.Value);
        
        return discountedPriceResult.IsSuccess ? discountedPriceResult.Value : Price;
    }

    private bool IsEnrollmentClosed(IDateTimeProvider dateTimeProvider)
    {
        return EnrollmentDeadline.HasValue && dateTimeProvider.UtcNow > EnrollmentDeadline.Value;
    }

    private bool HasStarted()
    {
        return Status == ClassStatus.InProgress || Status == ClassStatus.Completed;
    }

    private int GetStudentEnrollmentCount(StudentId studentId)
    {
        return _participants.Count(p => p == studentId) + _waitingList.Count(w => w == studentId);
    }

    private bool MeetsRequirements(StudentId studentId)
    {
        // Aqui seria necessário injetar um serviço para verificar os requisitos do estudante
        return true;
    }
    public Result Share(StudentId studentId, SocialNetwork network, Uri shareUrl)
    {
        if (Status != ClassStatus.Published)
            return Result.Invalid(new ValidationError("Apenas aulas publicadas podem ser compartilhadas"));

        var shareResult = SocialShare.Create(studentId, network, shareUrl);
        if (!shareResult.IsSuccess)
            return Result.Invalid(shareResult.ValidationErrors);

        _shares.Add(shareResult.Value);
        AddDomainEvent(new GroupClassSharedEvent(Id.Value, studentId.Value, network.ToString()));

        return Result.Success();
    }

    public Uri GenerateSharingLink()
    {
        // Na implementação real, isso seria configurado via injeção de dependência
        var baseUrl = "https://profhub.com.br/classes/";
        return new Uri($"{baseUrl}{Slug}");
    }

    public Result<SocialShare> GetLastShareByStudent(StudentId studentId)
    {
        var lastShare = _shares
            .Where(s => s.SharedBy == studentId)
            .OrderByDescending(s => s.SharedAt)
            .FirstOrDefault();

        return lastShare != null
            ? Result.Success(lastShare)
            : Result.NotFound<SocialShare>();
    }

    public IEnumerable<SocialShare> GetSharesByNetwork(SocialNetwork network)
        => _shares.Where(s => s.Network == network).OrderByDescending(s => s.SharedAt);
}
