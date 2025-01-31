using MediatR;
using Prof.Hub.Application.UseCases.Student.DeleteStudent;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.WebApi.Endpoints.Student.DeleteStudent;

public class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete($"/api/v{ApiVersions.V1}/students/{{id:guid}}",
            async (Guid id, IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteStudentInput(id), ct);

                return result switch
                {
                    { IsSuccess: true } => Results.NoContent(),
                    { Status: ResultStatus.NotFound } => Results.NotFound(result.ValidationErrors),
                    _ => Results.BadRequest()
                };
            });
    }
}
