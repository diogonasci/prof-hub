using MediatR;
using Prof.Hub.Application.Interfaces.Repositories;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Application.UseCases.Student.GetStudentById;
internal class GetStudentById : IRequestHandler<GetStudentByIdInput, Result<Domain.Aggregates.Student.Student>>
{
    private readonly IStudentRepository _studentRepository;

    public GetStudentById(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<Result<Domain.Aggregates.Student.Student>> Handle(GetStudentByIdInput input, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(input.Id);
        if (student is null)
        {
            return Result.NotFound($"Student with ID {input.Id} not found.");
        }

        return Result.Success(student);
    }
}
