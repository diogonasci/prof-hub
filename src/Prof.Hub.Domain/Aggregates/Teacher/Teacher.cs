using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Teacher;

public class Teacher : AuditableEntity, IAggregateRoot
{
    private readonly List<Qualification> _qualifications = [];
    private readonly List<TimeSlot> _availability = [];
    private readonly List<string> _specialties = [];

    private readonly IDateTimeProvider _dateTimeProvider;

    public TeacherId Id { get; private set; }
    public TeacherProfile Profile { get; private set; }
    public Rating Rating { get; private set; }
    public TeacherStatus Status { get; private set; }
    public decimal HourlyRate { get; private set; }
    public DateTime? LastActiveAt { get; private set; }

    public IReadOnlyList<Qualification> Qualifications => _qualifications.AsReadOnly();
    public IReadOnlyList<TimeSlot> Availability => _availability.AsReadOnly();
    public IReadOnlyList<string> Specialties => _specialties.AsReadOnly();

    private Teacher(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public static Result<Teacher> Create(string name, string bio, string avatarUrl, decimal hourlyRate, IDateTimeProvider dateTimeProvider)
    {
        var profileResult = TeacherProfile.Create(name, bio, avatarUrl);

        var errors = new List<ValidationError>();

        if (hourlyRate <= 0)
            errors.Add(new ValidationError("Valor da hora aula deve ser maior do que zero."));

        if (profileResult.ValidationErrors.Any())
            errors.AddRange(profileResult.ValidationErrors);

        if (errors.Count > 0)
            return Result.Invalid(errors);

        var teacher = new Teacher(dateTimeProvider)
        {
            Id = TeacherId.Create(),
            Profile = profileResult.Value,
            Status = TeacherStatus.Pending,
            HourlyRate = hourlyRate
        };

        return teacher;
    }

    public Result<bool> UpdateProfile(TeacherProfile newProfile)
    {
        Profile = newProfile;
        return true;
    }

    public Result<bool> AddQualification(Qualification qualification)
    {
        if (_qualifications.Any(q =>
            q.Title == qualification.Title &&
            q.Institution == qualification.Institution))
        {
            return Result.Invalid(new List<ValidationError>
            {
                new("Está qualificação já existe.")
            });
        }

        _qualifications.Add(qualification);

        return true;
    }

    public Result<bool> AddAvailability(TimeSlot timeSlot)
    {
        if (_availability.Any(a => a.Overlaps(timeSlot)))
        {
            return Result<bool>.Invalid(new List<ValidationError>
            {
                new("Este horário entra em conflito com uma disponibilidade existente.")
            });
        }

        _availability.Add(timeSlot);
        return Result<bool>.Success(true);
    }

    public Result<bool> AddSpecialty(string specialty)
    {
        if (string.IsNullOrWhiteSpace(specialty))
            return Result<bool>.Invalid(new List<ValidationError>
            {
                new("Especialidade deve ser informada.")
            });

        if (_specialties.Contains(specialty))
            return Result<bool>.Invalid(new List<ValidationError>
            {
                new("Está especialidade já existe.")
            });

        _specialties.Add(specialty);
        return Result<bool>.Success(true);
    }

    public Result<bool> UpdateRating(Rating rating)
    {
        Rating = rating;
        return Result<bool>.Success(true);
    }

    public Result<bool> UpdateStatus(TeacherStatus newStatus)
    {
        Status = newStatus;
        if (newStatus == TeacherStatus.Active)
            LastActiveAt = _dateTimeProvider.UtcNow;

        return Result<bool>.Success(true);
    }

    public Result<bool> UpdateHourlyRate(decimal newRate)
    {
        if (newRate <= 0)
            return Result<bool>.Invalid(new List<ValidationError>
            {
                new("O valor da hora aula deve ser maior do que zero.")
            });

        HourlyRate = newRate;
        return Result<bool>.Success(true);
    }
}
