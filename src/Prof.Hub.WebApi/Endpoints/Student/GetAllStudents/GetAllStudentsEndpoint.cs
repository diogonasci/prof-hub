using MediatR;
using Prof.Hub.Application.UseCases.Student.GetAllStudents;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.WebApi.Endpoints.Student.GetAllStudents;

public class GetAllStudents : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/students", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllStudentsInput(), ct);

            return result switch
            {
                { IsSuccess: true } => Results.Ok(new GetAllStudentsResponse(result.Value.Select(StudentDto.FromEntity))),
                { Status: ResultStatus.NotFound } => Results.NotFound(result.ValidationErrors),
                _ => Results.BadRequest(result.ValidationErrors)
            };
        });
    }
}
