using Microsoft.Extensions.DependencyInjection;
using Prof.Hub.Application.Interfaces;
using Prof.Hub.Infrastructure.ApiClients;

namespace Prof.Hub.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IJokeApiClient, JokeApiClient>();

            services.AddHttpClient<IJokeApiClient, JokeApiClient>();

            return services;
        }
    }
}
