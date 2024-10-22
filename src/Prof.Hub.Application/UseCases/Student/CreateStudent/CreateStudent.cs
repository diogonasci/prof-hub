using MediatR;
using Prof.Hub.Application.Interfaces.Repositories;
using Prof.Hub.Domain.Aggregates.Common;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Application.UseCases.Student.CreateStudent
{
    internal class CreateStudent : IRequestHandler<CreateStudentInput, Result<Domain.Aggregates.Student.Student>>
    {
        private readonly IStudentRepository _studentRepository;

        public CreateStudent(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<Result<Domain.Aggregates.Student.Student>> Handle(CreateStudentInput input, CancellationToken cancellationToken)
        {
            var valueObjectsResult = TryCreateValueObjects(input);
            if (!valueObjectsResult.IsSuccess)
                return Result.Invalid(valueObjectsResult.ValidationErrors);

            var (name, email, phoneNumber, address, classHours) = valueObjectsResult.Value;

            var student = Domain.Aggregates.Student.Student.Create(
                name,
                email,
                phoneNumber,
                address,
                null,
                classHours
            );

            await _studentRepository.AddAsync(student);

            return Result.Created(student, $"/api/v1/students/{student.Id}");
        }

        private static Result<(Name, Email, PhoneNumber, Address, ClassHours)> TryCreateValueObjects(CreateStudentInput input)
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
}
