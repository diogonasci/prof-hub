using MediatR;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Application.UseCases.Student.GetAllStudents;
public record GetAllStudentsInput : IRequest<Result<List<Domain.Aggregates.Student.Student>>>;
