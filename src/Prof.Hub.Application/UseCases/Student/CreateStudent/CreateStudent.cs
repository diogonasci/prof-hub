using MediatR;
using Prof.Hub.Application.Interfaces.Repositories;
using Prof.Hub.Application.Results;

namespace Prof.Hub.Application.UseCases.Student.CreateStudent
{
    internal class CreateStudent : IRequestHandler<CreateStudentInput, Result<Domain.Entities.Student>>
    {
        private readonly IStudentRepository _studentRepository;

        public CreateStudent(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<Result<Domain.Entities.Student>> Handle(CreateStudentInput input, CancellationToken cancellationToken)
        {
            var student = input.ToStudent();

            await _studentRepository.AddAsync(student);

            return Result.Created(student, $"/api/v1/students/{student.Id}");
        }
    }
}
