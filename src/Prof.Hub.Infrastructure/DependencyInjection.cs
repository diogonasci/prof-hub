using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Prof.Hub.Infrastructure.ApiClients;
using Prof.Hub.Infrastructure.PostgresSql.Configurations;
using Prof.Hub.Infrastructure.PostgresSql;
using Prof.Hub.Application.Interfaces.External;
using Prof.Hub.Application.Interfaces.Repositories;
using Prof.Hub.Infrastructure.Repositories;

namespace Prof.Hub.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>((serviceProvider, dbContextOptionsBuilder) =>
            {
                var dataBaseSettings = serviceProvider.GetService<IOptions<PostgreSqlSettings>>()!.Value;
                
                dbContextOptionsBuilder.UseNpgsql(dataBaseSettings.ConnectionString);
            });

            services.AddScoped<IStudentRepository, StudentRepository>();


            services.AddScoped<IJokeApiClient, JokeApiClient>();
            services.AddHttpClient<IJokeApiClient, JokeApiClient>();

            return services;
        }
    }
}
