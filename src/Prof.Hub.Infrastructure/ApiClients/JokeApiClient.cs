using Microsoft.Extensions.Options;
using Prof.Hub.Application.Interfaces;
using Prof.Hub.Domain.DTOs;
using Prof.Hub.Infrastructure.ApiClients.Configurations;
using System.Net.Http.Json;

namespace Prof.Hub.Infrastructure.ApiClients
{
    public class JokeApiClient : IJokeApiClient
    {
        private readonly HttpClient _httpClient;
        
        public JokeApiClient(HttpClient httpClient, IOptions<ExternalServicesSettings> options)
        {
            httpClient.BaseAddress = new Uri(options.Value.JokeApiBaseAddress);
            _httpClient = httpClient;
        }

        /// <summary>
        /// Obtém uma piada aleatória do serviço de piadas.
        /// </summary>
        /// <returns>Um objeto JokeDTO representando uma piada, ou null em caso de falha.</returns>
        public async Task<JokeDTO?> GetRandomJokeAsync()
        {
            return await _httpClient.GetFromJsonAsync<JokeDTO>("random_joke");
        }
    }
}
