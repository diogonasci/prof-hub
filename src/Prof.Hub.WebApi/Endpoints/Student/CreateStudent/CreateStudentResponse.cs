using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.WebApi.Endpoints.Student.CreateStudent;

public record CreateStudentResponse(
    string Id,
    string Name,
    string Email,
    string PhoneNumber,
    Grade? Grade)
{
    public static CreateStudentResponse FromResult(Result<Domain.Aggregates.Student.Student> result)
    {
        var student = result.Value;
        return new CreateStudentResponse(
            student.Id.Value,
            student.Profile.Name,
            student.Profile.Email.Value,
            student.Profile.PhoneNumber.Value,
            student.Profile.Grade);
    }
}

