using Prof.Hub.Domain.DTOs;

namespace Prof.Hub.Application.Interfaces
{
    public interface IJokeApiClient
    {
        Task<JokeDTO?> GetRandomJokeAsync();
    }
}
