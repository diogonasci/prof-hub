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

        var updatedStudent = studentResult.Value;

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
}
