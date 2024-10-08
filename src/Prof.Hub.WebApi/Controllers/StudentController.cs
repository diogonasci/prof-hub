using Microsoft.AspNetCore.Mvc;
using Prof.Hub.Application.Results;
using Prof.Hub.WebApi.Transport.CreateStudent;

namespace Prof.Hub.WebApi.Controllers
{
    [Route("api/v1/student")]
    public sealed class CreateStudentController : BaseController
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateStudent(CreateStudentRequest request, CancellationToken ct)
        {
            var result = await Mediator.Send(request.ToInput(), ct);

            return result switch
            {
                { IsSuccess: true } => Ok(result.Value),
                { Status: ResultStatus.Invalid } => BadRequest(result.ValidationErrors),
                _ => BadRequest()
            };
        }
    }
}
