using MediatR;
using Prof.Hub.Application.Interfaces.Repositories;
using Prof.Hub.Domain.Aggregates.Common;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Application.UseCases.Student.UpdateStudent;

internal class UpdateStudent : IRequestHandler<UpdateStudentInput, Result>
{
    private readonly IStudentRepository _studentRepository;

    public UpdateStudent(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<Result> Handle(UpdateStudentInput input, CancellationToken cancellationToken)
    {
        var existingStudent = await _studentRepository.GetByIdAsync(input.Id);
        if (existingStudent is null)
            return Result.NotFound($"Student with ID {input.Id} not found.");

        var valueObjectsResult = TryCreateValueObjects(input);
        if (!valueObjectsResult.IsSuccess)
            return Result.Invalid(valueObjectsResult.ValidationErrors);

        var (name, email, phoneNumber, address, classHours) = valueObjectsResult.Value;

        var updatedStudent = Domain.Aggregates.Student.Student.Create(
            name, email, phoneNumber, address,
            existingStudent.Parent, classHours
        );

        foreach (var lesson in existingStudent.PrivateLessons)
        {
            var result = updatedStudent.SchedulePrivateLesson(lesson);
            if (!result.IsSuccess)
                return result;
        }

        foreach (var lesson in existingStudent.GroupLessons)
        {
            var result = updatedStudent.JoinGroupLesson(lesson);
            if (!result.IsSuccess)
                return result;
        }

        await _studentRepository.UpdateAsync(updatedStudent);
        return Result.Success();
    }

    private static Result<(Name, Email, PhoneNumber, Address, ClassHours)> TryCreateValueObjects(UpdateStudentInput input)
    {
        var nameResult = Name.Create(input.Name);
        var emailResult = Email.Create(input.Email);
        var phoneResult = PhoneNumber.Create(input.PhoneNumber);
        var addressResult = Address.Create(input.Street, input.City, input.State, input.PostalCode);
        var classHoursResult = ClassHours.Create(input.ClassHours);

        if (!nameResult.IsSuccess || !emailResult.IsSuccess ||
            !phoneResult.IsSuccess || !addressResult.IsSuccess ||
            !classHoursResult.IsSuccess)
        {
            var errors = new List<ValidationError>();
            if (nameResult.ValidationErrors.Any()) errors.AddRange(nameResult.ValidationErrors);
            if (emailResult.ValidationErrors.Any()) errors.AddRange(emailResult.ValidationErrors);
            if (phoneResult.ValidationErrors.Any()) errors.AddRange(phoneResult.ValidationErrors);
            if (addressResult.ValidationErrors.Any()) errors.AddRange(addressResult.ValidationErrors);
            if (classHoursResult.ValidationErrors.Any()) errors.AddRange(classHoursResult.ValidationErrors);

            return Result.Invalid(errors);
        }

        return Result.Success((
            nameResult.Value,
            emailResult.Value,
            phoneResult.Value,
            addressResult.Value,
            classHoursResult.Value
        ));
    }
}
