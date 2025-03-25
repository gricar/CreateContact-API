using CreateContact.Application.Common.Messaging;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.RabbitMq;

namespace CreateContact.API.IntegrationTests.Infrastructure;

public class ApiFactory : WebApplicationFactory<IApiAssemblyMarker>, IAsyncLifetime
{
    private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5672))
        .Build();

    public RabbitMqConsumer RabbitMqConsumer;

    public async Task InitializeAsync()
    {
        await _rabbitMqContainer.StartAsync();
        RabbitMqConsumer = new RabbitMqConsumer(_rabbitMqContainer.GetConnectionString());
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Remove o RabbitMq configurado pela aplicação
            services.Remove<IEventBus>();

            // Registra o TestContainer usando a connection string do Testcontainers
            services.AddSingleton<IEventBus>(sp =>
            {
                //var logger = sp.GetRequiredService<ILogger<RabbitMQEventBus>>();
                return new RabbitMQEventBus(_rabbitMqContainer.GetConnectionString(), "test-container");
            });
        });

        base.ConfigureWebHost(builder);
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        return _rabbitMqContainer.DisposeAsync().AsTask();
    }
}
