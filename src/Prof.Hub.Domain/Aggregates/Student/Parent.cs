using System.Collections.Generic;
using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Student;
public class Parent : AuditableEntity
{
    public Name Name { get; private set; }
    public Email Email { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }

    private Parent()
    {
    }

    public static Result<Parent> Create(
        string name,
        string email,
        string phoneNumber)
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

        var parent = new Parent
        {
            Id = Guid.NewGuid(),
            Name = nameResult.Value,
            Email = emailResult.Value,
            PhoneNumber = phoneResult.Value
        };

        return Result.Success(parent);
    }

    public void Update(Name name, Email email, PhoneNumber phoneNumber)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
    }
}

