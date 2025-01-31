using MediatR;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Application.UseCases.Student.DeleteStudent;
public record DeleteStudentInput(Guid Id) : IRequest<Result>;
