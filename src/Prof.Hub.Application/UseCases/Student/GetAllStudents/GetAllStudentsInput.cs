using MediatR;
using Prof.Hub.Application.Results;

namespace Prof.Hub.Application.UseCases.Student.GetAllStudents;
public record GetAllStudentsInput : IRequest<Result<List<Domain.Entities.Student>>>;
