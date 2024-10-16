using MediatR;
using Prof.Hub.Application.Interfaces.Repositories;
using Prof.Hub.Application.Results;

namespace Prof.Hub.Application.UseCases.Student.DeleteStudent;
internal class DeleteStudent : IRequestHandler<DeleteStudentInput, Result>
{
    private readonly IStudentRepository _studentRepository;

    public DeleteStudent(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<Result> Handle(DeleteStudentInput input, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(input.Id);
        if (student is null)
        {
            return Result.NotFound($"Student with ID {input.Id} not found.");
        }

        await _studentRepository.DeleteAsync(input.Id);
        return Result.Success();
    }
}
