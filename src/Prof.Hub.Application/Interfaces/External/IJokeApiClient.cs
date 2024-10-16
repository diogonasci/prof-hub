using Prof.Hub.Domain.DTOs;

namespace Prof.Hub.Application.Interfaces.External
{
    public interface IJokeApiClient
    {
        Task<JokeDTO?> GetRandomJokeAsync();
    }
}
