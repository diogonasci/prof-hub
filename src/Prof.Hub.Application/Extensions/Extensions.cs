using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Prof.Hub.Application.Behaviors;
using Prof.Hub.Application.UseCases.Student.CreateStudent;

namespace Prof.Hub.Application.Extensions
{
    public static class Extensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(CreateStudent).Assembly);
            });

            services.AddValidatorsFromAssemblyContaining<CreateStudentValidator>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        }
    }
}
