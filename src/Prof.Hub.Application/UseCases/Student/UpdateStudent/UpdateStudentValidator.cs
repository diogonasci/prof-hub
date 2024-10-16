using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Prof.Hub.Application.UseCases.Student.UpdateStudent;
public class UpdateStudentValidator : AbstractValidator<UpdateStudentInput>
{
    public UpdateStudentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.")
            .Must(BeAValidGuid).WithMessage("Id must be a valid GUID.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .Length(1, 50).WithMessage("First name must be between 1 and 50 characters long.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Length(1, 50).WithMessage("Last name must be between 1 and 50 characters long.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number must be in a valid format.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .Length(1, 100).WithMessage("Address must be between 1 and 100 characters long.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.")
            .Length(1, 50).WithMessage("City must be between 1 and 50 characters long.");

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("State is required.")
            .Length(2, 50).WithMessage("State must be between 2 and 50 characters long.");

        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("Postal code is required.")
            .Length(5, 10).WithMessage("Postal code must be between 5 and 10 characters long.");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
