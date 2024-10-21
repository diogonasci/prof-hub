using MediatR;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Application.UseCases.Student.CreateStudent
{
    public record CreateStudentInput(
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        string Street,
        string City,
        string State,
        string PostalCode,
        int ClassHours
    ) : IRequest<Result<Domain.Aggregates.Student.Student>>;
}
