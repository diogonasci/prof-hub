using Prof.Hub.Application.UseCases.Student.CreateStudent;

namespace Prof.Hub.WebApi.Endpoints.Student.CreateStudent
{
    public record CreateStudentRequest(
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
        public CreateStudentInput ToInput()
        {
            return new CreateStudentInput(
                Name,
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
