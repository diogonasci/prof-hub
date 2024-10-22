using MediatR;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.WebApi.Endpoints.Student.UpdateStudent;

public class Put : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/students/{id:guid}", async (Guid id, UpdateStudentRequest request, IMediator mediator, CancellationToken ct) =>
        {
            var updatedRequest = request with { Id = id };
            var result = await mediator.Send(updatedRequest.ToInput(), ct);

            return result switch
            {
                { IsSuccess: true } => Results.NoContent(),
                { Status: ResultStatus.NotFound } => Results.NotFound(result.ValidationErrors),
                _ => Results.BadRequest()
            };
        });
    }
}
