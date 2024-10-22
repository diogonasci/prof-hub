using MediatR;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Application.UseCases.Student.UpdateStudent;
public record UpdateStudentInput(
        Guid Id,
        string Name,
        string Email,
        string PhoneNumber,
        string Street,
        string City,
        string State,
        string PostalCode,
        int ClassHours
    ) : IRequest<Result>;
