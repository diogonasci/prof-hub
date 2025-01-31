using MediatR;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Application.UseCases.Student.GetStudentById;
public record GetStudentByIdInput(Guid Id) : IRequest<Result<Domain.Aggregates.Student.Student>>;
