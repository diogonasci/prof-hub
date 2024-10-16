using MediatR;
using Prof.Hub.Application.Interfaces.Repositories;
using Prof.Hub.Application.Results;

namespace Prof.Hub.Application.UseCases.Student.GetStudent;
internal class GetStudent : IRequestHandler<GetStudentInput, Result<Domain.Entities.Student>>
{
    private readonly IStudentRepository _studentRepository;

    public GetStudent(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<Result<Domain.Entities.Student>> Handle(GetStudentInput input, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(input.Id);
        if (student is null)
        {
            return Result.NotFound($"Student with ID {input.Id} not found.");
        }

        return Result.Success(student);
    }
}
