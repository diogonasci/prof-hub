using Prof.Hub.Application.UseCases.Student.UpdateStudent;

namespace Prof.Hub.WebApi.Endpoints.Student.UpdateStudent;
public record UpdateStudentRequest(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        string Address,
        string City,
        string State,
        string PostalCode,
        int ClassHours
    )
{
    public UpdateStudentInput ToInput()
    {
        return new UpdateStudentInput(
            Id,
            FirstName,
            LastName,
            Email,
            PhoneNumber,
            Address,
            City,
            State,
            PostalCode,
            ClassHours
        );
    }
}
