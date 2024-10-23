using MediatR;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.WebApi.Endpoints.Student.CreateStudent;

public class Post : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost($"/api/v{ApiVersions.V1}/students", async (CreateStudentRequest request, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(request.ToInput(), ct);

            return result switch
            {
                { IsSuccess: true } => Results.Created(result.Location, CreateStudentResponse.FromEntity(result.Value)),
                { Status: ResultStatus.Invalid } => Results.BadRequest(result.ValidationErrors),
                _ => Results.BadRequest()
            };
        });
    }
}
