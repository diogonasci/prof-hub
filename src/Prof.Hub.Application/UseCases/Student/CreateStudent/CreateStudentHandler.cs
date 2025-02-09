using MediatR;
using Prof.Hub.Application.Interfaces.Repositories;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Application.UseCases.Student.CreateStudent;
internal class CreateStudentHandler : IRequestHandler<CreateStudentCommand, Result<Domain.Aggregates.Student.Student>>
{
    private readonly IStudentRepository _studentRepository;

    public CreateStudentHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<Result<Domain.Aggregates.Student.Student>> Handle(
        CreateStudentCommand command,
        CancellationToken cancellationToken)
    {
        var existingStudent = await _studentRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (existingStudent.IsSuccess)
            return Result.Invalid(new ValidationError("Email já está em uso"));

        var studentResult = Domain.Aggregates.Student.Student.Create(
            command.Name,
            command.Email,
            command.PhoneNumber);

        if (!studentResult.IsSuccess)
            return Result.Invalid(studentResult.ValidationErrors);

        var student = studentResult.Value;

        await _studentRepository.AddAsync(student, cancellationToken);

        return Result.Created(student, $"/api/v1/students/{student.Id.Value}");
    }
}
