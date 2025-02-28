using Prof.Hub.Application.UseCases.Student.CreateStudent;
using Prof.Hub.Domain.Enums;

namespace Prof.Hub.WebApi.Endpoints.Student.CreateStudent;

public record CreateStudentRequest(
    string Name,
    string Email,
    string PhoneNumber,
    Grade? Grade)
{
    public CreateStudentCommand ToCommand() => new(
        Name,
        Email,
        PhoneNumber,
        Grade);
}
