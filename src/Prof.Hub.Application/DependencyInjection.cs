﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Prof.Hub.Application.Behaviors;

namespace Prof.Hub.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));

        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

        return services;
    }
}
