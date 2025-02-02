using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Student.ValueObjects;
public sealed record StudentProfile
{
    public Name Name { get; private set; }
    public Email Email { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public Grade Grade { get; private set; }
    public Uri AvatarUrl { get; private set; }
    public ReferralCode ReferralCode { get; private set; }

    private StudentProfile()
    {
    }

    public static Result<StudentProfile> Create(
            string name,
            string email,
            string phoneNumber
        )
    {
        var nameResult = Name.Create(name);
        var emailResult = Email.Create(email);
        var phoneResult = PhoneNumber.Create(phoneNumber);

        if (!nameResult.IsSuccess || !emailResult.IsSuccess || !phoneResult.IsSuccess)
        {
            var errors = new List<ValidationError>();

            if (nameResult.ValidationErrors.Any()) errors.AddRange(nameResult.ValidationErrors);
            if (emailResult.ValidationErrors.Any()) errors.AddRange(emailResult.ValidationErrors);
            if (phoneResult.ValidationErrors.Any()) errors.AddRange(phoneResult.ValidationErrors);

            return Result.Invalid(errors);
        }

        var student = new StudentProfile
        {
            Name = nameResult.Value,
            Email = emailResult.Value,
            PhoneNumber = phoneResult.Value,
        };

        return Result.Success(student);
    }
}
