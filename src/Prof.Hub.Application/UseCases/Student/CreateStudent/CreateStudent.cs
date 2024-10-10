using MediatR;
using Prof.Hub.Application.Interfaces;
using Prof.Hub.Application.Results;

namespace Prof.Hub.Application.UseCases.Student.CreateStudent
{
    internal class CreateStudent : IRequestHandler<CreateStudentInput, Result<Domain.Entities.Student>>
    {
        private readonly IJokeApiClient _jokeApiClient;

        public CreateStudent(IJokeApiClient jokeApiClient)
        {
            _jokeApiClient = jokeApiClient;
        }

        public async Task<Result<Domain.Entities.Student>> Handle(CreateStudentInput input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input.FirstName))
            {
                return Result.Invalid(new ValidationError("Nome deve ser preenchido."));
            }

            if (string.IsNullOrEmpty(input.Email))
            {
                return Result.Invalid(new List<ValidationError> {
                    new() { ErrorMessage = "Email cannot be empty" }
                });
            }

            var jokeResult = await _jokeApiClient.GetRandomJokeAsync();
            if (jokeResult is null)
            {
                return Result.Invalid(new List<ValidationError> {
                    new() { ErrorMessage = "Failed to fetch a joke." }
                });
            }

            var student = input.ToStudent();
            return await Task.FromResult(Result.Success(student));
        }
    }
}
