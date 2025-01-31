using MediatR;
using Prof.Hub.Application.UseCases.Student.GetStudentById;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.WebApi.Endpoints.Student.GetStudentById;

public class GetStudentById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"/api/v{ApiVersions.V1}/students/{{id:guid}}",
            async (Guid id, IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetStudentByIdInput(id), ct);

                return result switch
                {
                    { IsSuccess: true } => Results.Ok(GetStudentByIdResponse.FromEntity(result.Value)),
                    { Status: ResultStatus.NotFound } => Results.NotFound(),
                    _ => Results.BadRequest()
                };
            });
    }
}
