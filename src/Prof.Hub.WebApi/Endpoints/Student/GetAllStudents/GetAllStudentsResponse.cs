namespace Prof.Hub.WebApi.Endpoints.Student.GetAllStudents;

public record GetAllStudentsResponse(
        IEnumerable<StudentDto> Students
    );

public record StudentDto(
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
    public static StudentDto FromEntity(Domain.Aggregates.Student.Student student)
    {
        return new StudentDto(
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
