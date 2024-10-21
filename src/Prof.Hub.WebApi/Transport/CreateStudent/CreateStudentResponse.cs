using Prof.Hub.Domain.Aggregates.Student;

namespace Prof.Hub.WebApi.Transport.CreateStudent;

public record CreateStudentResponse(
    Guid Id,
    string Name,
    string Email,
    string PhoneNumber,
    string Street,
    string City,
    string State,
    string PostalCode,
    int ClassHours
)
{
    public static CreateStudentResponse FromEntity(Student student)
    {
        return new CreateStudentResponse(
            student.Id,
            student.Name.Value,
            student.Email.Value,
            student.PhoneNumber.Value,
            student.Address.Street,
            student.Address.City,
            student.Address.State,
            student.Address.PostalCode,
            student.ClassHours.Value
        );
    }
}
