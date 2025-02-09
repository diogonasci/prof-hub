using MediatR;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Application.UseCases.Student.CreateStudent;

public record CreateStudentCommand(
    string Name,
    string Email,
    string PhoneNumber,
    Grade? Grade) : IRequest<Result<Domain.Aggregates.Student.Student>>;
