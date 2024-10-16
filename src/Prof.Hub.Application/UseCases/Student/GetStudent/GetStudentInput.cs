using MediatR;
using Prof.Hub.Application.Results;

namespace Prof.Hub.Application.UseCases.Student.GetStudent;
public record GetStudentInput(Guid Id) : IRequest<Result<Domain.Entities.Student>>;
