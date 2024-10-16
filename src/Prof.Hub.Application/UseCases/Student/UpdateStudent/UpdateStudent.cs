using MediatR;
using Prof.Hub.Application.Interfaces.Repositories;
using Prof.Hub.Application.Results;

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
        var student = await _studentRepository.GetByIdAsync(input.Id);
        if (student is null)
        {
            return Result.NotFound($"Student with ID {input.Id} not found.");
        }

        student.FirstName = input.FirstName;
        student.LastName = input.LastName;
        student.Email = input.Email;
        student.PhoneNumber = input.PhoneNumber;
        student.Address = input.Address;
        student.City = input.City;
        student.State = input.State;
        student.PostalCode = input.PostalCode;

        await _studentRepository.UpdateAsync(student);
        return Result.Success();
    }
}
