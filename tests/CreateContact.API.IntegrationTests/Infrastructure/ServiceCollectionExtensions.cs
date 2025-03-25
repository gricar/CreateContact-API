using Microsoft.Extensions.DependencyInjection;

namespace CreateContact.API.IntegrationTests.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void Remove<T>(this IServiceCollection services)
    {
        // Remove o serviço configurado pela aplicação para subistiuir pelo test container
        var descriptor = services
            .SingleOrDefault(s => s.ServiceType == typeof(T));

        if (descriptor is not null)
        {
            services.Remove(descriptor);
        }
    }
}
