using CreateContact.Application.Common.Behaviors;
using CreateContact.Application.Common.Messaging;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CreateContact.Application;

public static class DependecyInjection
{
    public static IServiceCollection ConfigureApplicationServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = typeof(DependecyInjection).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        services.AddValidatorsFromAssembly(assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddSingleton<IEventBus>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<RabbitMQEventBus>>();
            var hostname = configuration["RabbitMQ:Host"]!;
            var connectionName = configuration["RabbitMQ:ConnectionName"]!;
            return new RabbitMQEventBus(hostname, connectionName, logger);
        });

        return services;
    }
}
