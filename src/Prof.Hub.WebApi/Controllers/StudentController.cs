using Microsoft.AspNetCore.Mvc;
using Prof.Hub.Application.Results;
using Prof.Hub.Application.UseCases.Student.DeleteStudent;
using Prof.Hub.Application.UseCases.Student.GetAllStudents;
using Prof.Hub.Application.UseCases.Student.GetStudent;
using Prof.Hub.WebApi.Transport.CreateStudent;
using Prof.Hub.WebApi.Transport.GetAllStudents;
using Prof.Hub.WebApi.Transport.GetStudentById;
using Prof.Hub.WebApi.Transport.UpdateStudent;

namespace Prof.Hub.WebApi.Controllers
{
    [Route("api/v1/students")]
    public sealed class StudentController : BaseController
    {
        [HttpPost()]
        public async Task<IActionResult> CreateStudent(CreateStudentRequest request, CancellationToken ct)
        {
            var result = await Mediator.Send(request.ToInput(), ct);

            return result switch
            {
                { IsSuccess: true } => Created(
                    $"/api/v1/students/{result.Value.Id}",
                    CreateStudentResponse.FromEntity(result.Value)
                ),
                { Status: ResultStatus.Invalid } => BadRequest(result.ValidationErrors),
                _ => BadRequest()
            };
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetStudentById(Guid id, CancellationToken ct)
        {
            var result = await Mediator.Send(new GetStudentInput(id), ct);

            return result switch
            {
                { IsSuccess: true } => Ok(GetStudentByIdResponse.FromEntity(result.Value)),
                { Status: ResultStatus.NotFound } => NotFound(),
                _ => BadRequest()
            };
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllStudents(CancellationToken ct)
        {
            var result = await Mediator.Send(new GetAllStudentsInput(), ct);

            return result switch
            {
                { IsSuccess: true } => Ok(new GetAllStudentsResponse(result.Value.Select(StudentDto.FromEntity))),
                { Status: ResultStatus.NotFound } => NotFound(result.ValidationErrors),
                _ => BadRequest(result.ValidationErrors)
            };
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateStudent(Guid id, UpdateStudentRequest request, CancellationToken ct)
        {
            var updatedRequest = request with { Id = id };
            var result = await Mediator.Send(updatedRequest.ToInput(), ct);

            return result switch
            {
                { IsSuccess: true } => NoContent(),
                { Status: ResultStatus.NotFound } => NotFound(result.ValidationErrors),
                _ => BadRequest()
            };
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteStudent(Guid id, CancellationToken ct)
        {
            var result = await Mediator.Send(new DeleteStudentInput(id), ct);

            return result switch
            {
                { IsSuccess: true } => NoContent(),
                { Status: ResultStatus.NotFound } => NotFound(result.ValidationErrors),
                _ => BadRequest()
            };
        }
    }
}
