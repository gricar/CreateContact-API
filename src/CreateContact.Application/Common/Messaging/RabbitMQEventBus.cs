using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace CreateContact.Application.Common.Messaging;

public class RabbitMQEventBus(
    string hostname,
    string connectionName,
    ILogger<RabbitMQEventBus> logger) : IEventBus
{
    public async Task PublishAsync<T>(T message, string queueName)
    {
        var factory = new ConnectionFactory();
        factory.HostName = hostname;
        factory.ClientProvidedName = connectionName;

        IConnection conn = await factory.CreateConnectionAsync();

        IChannel channel = await conn.CreateChannelAsync();

        await channel.QueueDeclareAsync(queueName, true, false, false, null);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        var props = new BasicProperties();

        props.Persistent = true;

        await channel.BasicPublishAsync(string.Empty, queueName, true, props, body);

        logger.LogInformation("Message published to queue {QueueName} with message: {Message}", queueName, message);

        await Task.CompletedTask;
    }
}
