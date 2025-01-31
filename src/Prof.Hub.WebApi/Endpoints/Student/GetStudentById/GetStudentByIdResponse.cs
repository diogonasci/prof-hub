namespace Prof.Hub.WebApi.Endpoints.Student.GetStudentById;
public record GetStudentByIdResponse(
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
    public static GetStudentByIdResponse FromEntity(Domain.Aggregates.Student.Student student)
    {
        return new GetStudentByIdResponse(
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
