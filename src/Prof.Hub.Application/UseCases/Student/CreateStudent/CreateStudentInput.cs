using MediatR;
using Prof.Hub.Application.Results;

namespace Prof.Hub.Application.UseCases.Student.CreateStudent
{
    public record CreateStudentInput(
        string FirstName,
        string LastName,
        DateTime DateOfBirth,
        string Email,
        string PhoneNumber,
        string Address,
        string City,
        string State,
        string PostalCode
    ) : IRequest<Result<Domain.Entities.Student>>
    {
        public Domain.Entities.Student ToStudent()
        {
            return new Domain.Entities.Student
            {
                Id = Guid.NewGuid(),
                FirstName = FirstName,
                LastName = LastName,
                DateOfBirth = DateOfBirth,
                Email = Email,
                PhoneNumber = PhoneNumber,
                Address = Address,
                City = City,
                State = State,
                PostalCode = PostalCode,
                IsActive = true
            };
        }
    }
}
