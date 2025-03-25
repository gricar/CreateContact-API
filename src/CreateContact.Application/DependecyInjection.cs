using CreateContact.Application.Common.Behaviors;
using CreateContact.Application.Common.Messaging;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            //var logger = sp.GetRequiredService<ILogger<RabbitMQEventBus>>();
            //var hostname = configuration["MessageBroker:Host"]!;
            var uri = configuration["MessageBroker:Uri"]!;
            var connectionName = configuration["MessageBroker:ConnectionName"]!;
            return new RabbitMQEventBus(uri, connectionName);
        });

        return services;
    }
}
