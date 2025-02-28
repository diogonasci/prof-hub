using MediatR;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.WebApi.Endpoints.Student.CreateStudent;

public class Post : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost($"/api/v{ApiVersions.V1}/students",
            async (CreateStudentRequest request, IMediator mediator, CancellationToken ct) =>
            {
                var command = request.ToCommand();
                var result = await mediator.Send(command, ct);

                return result.Status switch
                {
                    ResultStatus.Created => Results.Created(
                        result.Location,
                        CreateStudentResponse.FromResult(result)),
                    ResultStatus.Invalid => Results.BadRequest(result.ValidationErrors),
                    _ => Results.BadRequest(result.Errors)
                };
            })
            .WithName("CreateStudent")
            .WithTags("Students")
            .Produces<CreateStudentResponse>(201)
            .ProducesValidationProblem()
            .WithOpenApi();
    }
}
