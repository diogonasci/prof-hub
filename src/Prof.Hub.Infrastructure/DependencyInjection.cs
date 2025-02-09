using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prof.Hub.Application.Events.Student;
using Prof.Hub.Application.Interfaces.Repositories;
using Prof.Hub.Domain.Aggregates.GroupClass.Events;
using Prof.Hub.Infrastructure.Clock;
using Prof.Hub.Infrastructure.PostgresSql;
using Prof.Hub.Infrastructure.Repositories;
using Prof.Hub.Infrastructure.Services;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        AddPersistence(services, configuration);
        AddEventHandling(services);
        AddHealthChecks(services, configuration);
        AddApiVersioning(services);

        return services;
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection") ??
                                    throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());

        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IUnitOfWork, ApplicationDbContext>();
    }

    private static void AddEventHandling(IServiceCollection services)
    {
        services.AddMediatR(cfg => {
            // Pode usar qualquer tipo do assembly de Application, como um handler existente
            cfg.RegisterServicesFromAssembly(typeof(StudentEnrolledEventHandler).Assembly);

            // Pode usar qualquer tipo do assembly de Domain, como um evento existente
            cfg.RegisterServicesFromAssembly(typeof(StudentEnrolledEvent).Assembly);
        });

        services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
    }

    private static void AddHealthChecks(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("DefaultConnection")!);
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
