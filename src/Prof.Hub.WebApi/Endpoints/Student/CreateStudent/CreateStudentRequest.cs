using Prof.Hub.Application.UseCases.Student.CreateStudent;

namespace Prof.Hub.WebApi.Endpoints.Student.CreateStudent
{
    public record CreateStudentRequest(
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        string Street,
        string City,
        string State,
        string PostalCode,
        int ClassHours
    )
    {
        public CreateStudentInput ToInput()
        {
            return new CreateStudentInput(
                FirstName,
                LastName,
                Email,
                PhoneNumber,
                Street,
                City,
                State,
                PostalCode,
                ClassHours
            );
        }
    }
}
