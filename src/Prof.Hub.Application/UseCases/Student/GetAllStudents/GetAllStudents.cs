using MediatR;
using Prof.Hub.Application.Interfaces.Repositories;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Application.UseCases.Student.GetAllStudents;
internal class GetAllStudents : IRequestHandler<GetAllStudentsInput, Result<List<Domain.Aggregates.Student.Student>>>
{
    private readonly IStudentRepository _studentRepository;

    public GetAllStudents(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<Result<List<Domain.Aggregates.Student.Student>>> Handle(GetAllStudentsInput request, CancellationToken cancellationToken)
    {
        var students = await _studentRepository.GetAllAsync();
        return Result.Success(students);
    }
}
