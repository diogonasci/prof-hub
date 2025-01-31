using Microsoft.EntityFrameworkCore;
using Asp.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prof.Hub.Application.Interfaces.Repositories;
using Prof.Hub.Infrastructure.PostgresSql;
using Prof.Hub.Infrastructure.Repositories;
using Prof.Hub.SharedKernel;
using Prof.Hub.Infrastructure.Clock;

namespace Prof.Hub.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            AddPersistence(services, configuration);

            AddCaching(services, configuration);

            AddHealthChecks(services, configuration);

            AddApiVersioning(services);

            return services;
        }

        private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("Database") ??
                                      throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());

            services.AddScoped<IStudentRepository, StudentRepository>();

            services.AddScoped<IUnitOfWork, ApplicationDbContext>();
        }

        private static void AddCaching(IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("Cache") ??
                                      throw new ArgumentNullException(nameof(configuration));

            services.AddStackExchangeRedisCache(options => options.Configuration = connectionString);
        }

        private static void AddHealthChecks(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddNpgSql(configuration.GetConnectionString("Database")!)
                .AddRedis(configuration.GetConnectionString("Cache")!);
        }

        private static void AddApiVersioning(IServiceCollection services)
        {
            services
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1);
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = new UrlSegmentApiVersionReader();
                })
                .AddMvc()
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'V";
                    options.SubstituteApiVersionInUrl = true;
                });
        }
    }
}
