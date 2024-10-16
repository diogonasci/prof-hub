using MediatR;
using Prof.Hub.Application.Interfaces.Repositories;
using Prof.Hub.Application.Results;

namespace Prof.Hub.Application.UseCases.Student.GetAllStudents;
internal class GetAllStudents : IRequestHandler<GetAllStudentsInput, Result<List<Domain.Entities.Student>>>
{
    private readonly IStudentRepository _studentRepository;

    public GetAllStudents(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<Result<List<Domain.Entities.Student>>> Handle(GetAllStudentsInput request, CancellationToken cancellationToken)
    {
        var students = await _studentRepository.GetAllAsync();
        return Result.Success(students);
    }
}
