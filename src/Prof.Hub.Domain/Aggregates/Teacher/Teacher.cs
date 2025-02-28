using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Teacher.Events;
using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Teacher;

public class Teacher : AuditableEntity, IAggregateRoot
{
    private readonly List<Qualification> _qualifications = [];
    private readonly List<HourlyRate> _rateHistory = [];
    private readonly List<Specialty> _specialties = [];
    private const int MAX_SPECIALTIES = 3;
    private const int MAX_QUALIFICATIONS = 10;

    private readonly IDateTimeProvider _dateTimeProvider;

    public TeacherId Id { get; private set; }
    public TeacherProfile Profile { get; private set; }
    public Rating Rating { get; private set; }
    public TeacherStatus Status { get; private set; }
    public TeacherAvailability Availability { get; private set; }
    public HourlyRate CurrentRate => _rateHistory.MaxBy(r => r.EffectiveFrom)!;
    public DateTime? LastActiveAt { get; private set; }

    public IReadOnlyCollection<Qualification> Qualifications => _qualifications.AsReadOnly();
    public IReadOnlyCollection<HourlyRate> RateHistory => _rateHistory.AsReadOnly();
    public IReadOnlyCollection<Specialty> Specialties => _specialties.AsReadOnly();

    private Teacher(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        Rating = Rating.Create();
        Availability = TeacherAvailability.Create();
    }

    public static Result<Teacher> Create(
        string name,
        string bio,
        string avatarUrl,
        Money hourlyRate,
        IDateTimeProvider dateTimeProvider)
    {
        var profileResult = TeacherProfile.Create(name, bio, avatarUrl);
        var rateResult = HourlyRate.Create(hourlyRate, dateTimeProvider.UtcNow);

        var errors = new List<ValidationError>();

        if (!profileResult.IsSuccess)
            errors.AddRange(profileResult.ValidationErrors);

        if (!rateResult.IsSuccess)
            errors.AddRange(rateResult.ValidationErrors);

        if (errors.Count > 0)
            return Result.Invalid(errors);

        var teacher = new Teacher(dateTimeProvider)
        {
            Id = TeacherId.Create(),
            Profile = profileResult.Value,
            Status = TeacherStatus.Pending
        };

        teacher._rateHistory.Add(rateResult.Value);
        teacher.AddDomainEvent(new TeacherCreatedEvent(teacher.Id.Value));

        return teacher;
    }

    public Result UpdateHourlyRate(Money newRate)
    {
        var rateResult = HourlyRate.Create(newRate, _dateTimeProvider.UtcNow);
        if (!rateResult.IsSuccess)
            return Result.Invalid(rateResult.ValidationErrors);

        if (newRate.Amount < CurrentRate.Value.Amount)
            return Result.Invalid(new ValidationError("Novo valor não pode ser menor que o atual"));

        _rateHistory.Add(rateResult.Value);

        AddDomainEvent(new TeacherRateUpdatedEvent(
        Id.Value,
        rateResult.Value.Value.Amount,
        rateResult.Value.Value.Currency,
        _dateTimeProvider.UtcNow
        ));

        return Result.Success();
    }

    public Result<Specialty> AddSpecialty(SubjectArea area, string description)
    {
        if (_specialties.Count >= MAX_SPECIALTIES)
            return Result.Invalid(new ValidationError($"Máximo de {MAX_SPECIALTIES} especialidades permitido"));

        var specialtyResult = Specialty.Create(area, description);
        if (!specialtyResult.IsSuccess)
            return specialtyResult;

        if (_specialties.Any(s => s.Area == area))
            return Result.Invalid(new ValidationError("Já existe uma especialidade nesta área"));

        _specialties.Add(specialtyResult.Value);

        AddDomainEvent(new TeacherSpecialtyAddedEvent(
        Id.Value,
        specialtyResult.Value.Area.ToString(),
        specialtyResult.Value.Description,
        specialtyResult.Value.IsVerified
        ));

        return Result.Success();
    }

    public Result<Qualification> AddQualification(string title, string institution, DateTime obtainedAt)
    {
        if (_qualifications.Count >= MAX_QUALIFICATIONS)
            return Result.Invalid(new ValidationError($"Máximo de {MAX_QUALIFICATIONS} qualificações permitido"));

        var qualificationResult = Qualification.Create(title, institution, obtainedAt, _dateTimeProvider);
        if (!qualificationResult.IsSuccess)
            return qualificationResult;

        if (_qualifications.Any(q =>
            q.Title == qualificationResult.Value.Title &&
            q.Institution == qualificationResult.Value.Institution))
        {
            return Result.Invalid(new ValidationError("Esta qualificação já existe"));
        }

        _qualifications.Add(qualificationResult.Value);

        return Result.Success();
    }

    public Result UpdateStatus(TeacherStatus newStatus)
    {
        if (!IsValidStatusTransition(Status, newStatus))
            return Result.Invalid(new ValidationError("Transição de status inválida"));

        if (newStatus == TeacherStatus.Active && !MeetsActiveRequirements())
            return Result.Invalid(new ValidationError("Professor não atende requisitos mínimos para ficar ativo"));

        var previousStatus = Status;
        Status = newStatus;

        if (newStatus == TeacherStatus.Active)
            LastActiveAt = _dateTimeProvider.UtcNow;

        AddDomainEvent(new TeacherStatusChangedEvent(Id.Value, previousStatus.ToString(), newStatus.ToString()));
        return Result.Success();
    }

    public Result AddTimeSlot(TimeSlot timeSlot)
    {
        if (Status == TeacherStatus.Inactive || Status == TeacherStatus.Suspended)
            return Result.Invalid(new ValidationError("Professor inativo não pode adicionar horários"));

        var result = Availability.AddTimeSlot(timeSlot);
        if (result.IsSuccess)
            AddDomainEvent(new TeacherAvailabilityUpdatedEvent(Id.Value));

        return result;
    }

    public Result RemoveTimeSlot(TimeSlot timeSlot)
    {
        var result = Availability.RemoveTimeSlot(timeSlot);
        if (result.IsSuccess)
            AddDomainEvent(new TeacherAvailabilityUpdatedEvent(Id.Value));

        return result;
    }

    public Result AddRating(Rating rating)
    {
        var result = Rating.AddRating(rating);
        if (result.IsSuccess)
            AddDomainEvent(new TeacherRatingUpdatedEvent(Id.Value, Rating.AverageScore));

        return result;
    }

    private bool IsValidStatusTransition(TeacherStatus currentStatus, TeacherStatus newStatus)
    {
        return (currentStatus, newStatus) switch
        {
            (TeacherStatus.Pending, TeacherStatus.Active) => true,
            (TeacherStatus.Pending, TeacherStatus.Inactive) => true,
            (TeacherStatus.Active, TeacherStatus.Suspended) => true,
            (TeacherStatus.Active, TeacherStatus.Inactive) => true,
            (TeacherStatus.Suspended, TeacherStatus.Active) => true,
            (TeacherStatus.Suspended, TeacherStatus.Inactive) => true,
            (TeacherStatus.Inactive, TeacherStatus.Active) => true,
            _ => false
        };
    }

    private bool MeetsActiveRequirements()
    {
        return Profile != null &&
               _specialties.Any(s => s.IsVerified) &&
               _qualifications.Any(q => q.IsVerified) &&
               Availability.HasMinimumAvailability();
    }
}
