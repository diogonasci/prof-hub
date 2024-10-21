using Prof.Hub.Domain.Aggregates.Student;

namespace Prof.Hub.WebApi.Transport.GetAllStudents;

public record GetAllStudentsResponse(
        IEnumerable<StudentDto> Students
    );

public record StudentDto(
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
    public static StudentDto FromEntity(Student student)
    {
        return new StudentDto(
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
