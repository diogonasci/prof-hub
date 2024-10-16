using MediatR;
using Prof.Hub.Application.Results;

namespace Prof.Hub.Application.UseCases.Student.UpdateStudent;
public record UpdateStudentInput(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        string Address,
        string City,
        string State,
        string PostalCode
    ) : IRequest<Result>;
