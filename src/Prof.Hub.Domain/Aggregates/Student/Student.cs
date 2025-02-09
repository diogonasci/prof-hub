using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.Entities;
using Prof.Hub.Domain.Aggregates.Student.Events;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;


namespace Prof.Hub.Domain.Aggregates.Student;
public class Student : AuditableEntity, IAggregateRoot
{
    private readonly List<PrivateClass.PrivateClass> _privateClasses = [];
    private readonly List<GroupClass.GroupClass> _groupClasses = [];
    private readonly List<EnrollmentHistory> _enrollmentHistory = [];
    private readonly List<TeacherFavorite> _favoriteTeachers = [];

    public StudentId Id { get; private set; }
    public StudentProfile Profile { get; private set; }
    public Balance Balance { get; private set; }
    public School? School { get; private set; }

    public IReadOnlyList<PrivateClass.PrivateClass> PrivateClasses => _privateClasses.AsReadOnly();
    public IReadOnlyList<GroupClass.GroupClass> GroupClasses => _groupClasses.AsReadOnly();
    public IReadOnlyList<EnrollmentHistory> EnrollmentHistory => _enrollmentHistory.AsReadOnly();
    public IReadOnlyList<TeacherFavorite> FavoriteTeachers => _favoriteTeachers.AsReadOnly();

    private Student()
    {
    }

    public static Result<Student> Create(
        string name,
        string email,
        string phoneNumber)
    {
        var profileResult = StudentProfile.Create(name, email, phoneNumber);
        var balanceResult = Balance.Create(0);

        var errors = new List<ValidationError>();

        if (!profileResult.IsSuccess)
            errors.AddRange(profileResult.ValidationErrors);

        if (!balanceResult.IsSuccess)
            errors.AddRange(balanceResult.ValidationErrors);

        if (errors.Count > 0)
            return Result.Invalid(errors);

        var student = new Student
        {
            Id = StudentId.Create(),
            Profile = profileResult.Value,
            Balance = balanceResult.Value,
        };

        return student;
    }

    public Result UpdateProfile(StudentProfile profile)
    {
        Profile = profile;
        AddDomainEvent(new StudentProfileUpdatedEvent(
            Id.Value,
            profile.Name,
            profile.Email.Value,
            profile.PhoneNumber.Value,
            profile.Grade.ToString(),
            profile.AvatarUrl?.ToString(),
            profile.ReferralCode.Value
        ));
        return Result.Success();
    }

    public Result UpdateSchool(School school)
    {
        School = school;

        AddDomainEvent(new StudentSchoolUpdatedEvent(
            Id.Value,
            school.Name,
            school.City,
            school.State,
            school.IsVerified
        ));

        return Result.Success();
    }

    public Result AddToFavorites(TeacherId teacherId, string? note = null)
    {
        if (_favoriteTeachers.Any(f => f.TeacherId == teacherId))
            return Result.Invalid(new ValidationError("Professor já está nos favoritos."));

        var favorite = new TeacherFavorite(Id, teacherId, note);
        _favoriteTeachers.Add(favorite);

        return Result.Success();
    }

    public Result RemoveFromFavorites(TeacherId teacherId)
    {
        var favorite = _favoriteTeachers.FirstOrDefault(f => f.TeacherId == teacherId);
        if (favorite == null)
            return Result.Invalid(new ValidationError("Professor não está nos favoritos."));

        _favoriteTeachers.Remove(favorite);
        AddDomainEvent(new TeacherRemovedFromFavoritesEvent(Id.Value, teacherId.Value));

        return Result.Success();
    }

    public Result UpdateFavoriteNote(TeacherId teacherId, string note)
    {
        var favorite = _favoriteTeachers.FirstOrDefault(f => f.TeacherId == teacherId);
        if (favorite == null)
            return Result.Invalid(new ValidationError("Professor não está nos favoritos."));

        favorite.UpdateNote(note);

        return Result.Success();
    }

    public Result AddBalance(Money amount)
    {
        var result = Balance.Add(amount);
        if (!result.IsSuccess)
            return Result.Invalid(result.ValidationErrors);

        Balance = result.Value;
        AddDomainEvent(new StudentBalanceUpdatedEvent(Id.Value, Balance.Amount.Amount));

        return Result.Success();
    }

    public Result DeductBalance(Money amount)
    {
        var result = Balance.Subtract(amount);
        if (!result.IsSuccess)
            return Result.Invalid(result.ValidationErrors);

        Balance = result.Value;
        AddDomainEvent(new StudentBalanceUpdatedEvent(Id.Value, Balance.Amount.Amount));

        return Result.Success();
    }

    public Result AddToEnrollmentHistory(EnrollmentHistory enrollment)
    {
        _enrollmentHistory.Add(enrollment);

        return Result.Success();
    }

    public Result EnrollInPrivateClass(PrivateClass.PrivateClass privateClass)
    {
        _privateClasses.Add(privateClass);
        var enrollment = Entities.EnrollmentHistory.CreateForPrivateClass(Id, privateClass.Id, privateClass.GetStatus());
        AddToEnrollmentHistory(enrollment);

        return Result.Success();
    }

    public Result EnrollInGroupClass(GroupClass.GroupClass groupClass)
    {
        var errors = new List<ValidationError>();

        if (Balance.Amount < groupClass.Price.Value)
            errors.Add(new ValidationError("Saldo insuficiente para esta aula."));

        if (_groupClasses.Any(c => c.Schedule.Overlaps(groupClass.Schedule)))
            errors.Add(new ValidationError("Existe conflito de horário com outra aula."));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        _groupClasses.Add(groupClass);
        var enrollment = Entities.EnrollmentHistory.CreateForGroupClass(Id, groupClass.Id, groupClass.GetStatus());
        AddToEnrollmentHistory(enrollment);

        var deductionResult = DeductBalance(groupClass.Price);
        if (!deductionResult.IsSuccess)
            return deductionResult;

        AddDomainEvent(new StudentEnrolledInGroupClassEvent(Id.Value, groupClass.Id.Value));

        return Result.Success();
    }
}
