using Prof.Hub.Domain.Aggregates.Student;

namespace Prof.Hub.WebApi.Transport.GetStudentById;
public record GetStudentByIdResponse(
        Guid Id,
        string FirstName,
        string LastName,
        DateTime DateOfBirth,
        string Email,
        string PhoneNumber,
        string Address,
        string City,
        string State,
        string PostalCode,
        bool IsActive
    )
{
    public static GetStudentByIdResponse FromEntity(Student student)
    {
        return new GetStudentByIdResponse(
            student.Id,
            student.FirstName,
            student.LastName,
            student.DateOfBirth,
            student.Email,
            student.PhoneNumber,
            student.Address,
            student.City,
            student.State,
            student.PostalCode,
            student.IsActive
        );
    }
}
