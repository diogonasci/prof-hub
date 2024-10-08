using Prof.Hub.Application.UseCases.Student.CreateStudent;

namespace Prof.Hub.WebApi.Transport.CreateStudent
{
    public record CreateStudentRequest(
        string FirstName,
        string LastName,
        DateTime DateOfBirth,
        string Email,
        string PhoneNumber,
        string Address,
        string City,
        string State,
        string PostalCode
    )
    {
        public CreateStudentInput ToInput()
        {
            return new CreateStudentInput(
                FirstName,
                LastName,
                DateOfBirth,
                Email,
                PhoneNumber,
                Address,
                City,
                State,
                PostalCode
            );
        }
    }
}
