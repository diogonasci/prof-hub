using MediatR;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Application.UseCases.Student.GetStudent;
public record GetStudentInput(Guid Id) : IRequest<Result<Domain.Aggregates.Student.Student>>;
