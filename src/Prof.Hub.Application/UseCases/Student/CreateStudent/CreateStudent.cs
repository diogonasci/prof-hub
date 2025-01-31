using MediatR;
using Prof.Hub.Application.Interfaces.Repositories;
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

        public async Task<Result<Domain.Aggregates.Student.Student>> Handle(CreateStudentInput input,
            CancellationToken cancellationToken)
        {
            var studentResult = Domain.Aggregates.Student.Student.Create(
                input.Name,
                input.Email,
                input.PhoneNumber,
                input.Street,
                input.City,
                input.State,
                input.PostalCode,
                input.ClassHours
            );

            if (!studentResult.IsSuccess)
                return Result.Invalid(studentResult.ValidationErrors);

            var student = studentResult.Value;

            await _studentRepository.AddAsync(student);

            return Result.Created(student, $"/api/v1/students/{student.Id}");
        }
    }
}
