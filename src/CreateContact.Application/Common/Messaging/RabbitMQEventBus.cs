using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace CreateContact.Application.Common.Messaging;

public class RabbitMQEventBus : IEventBus
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<RabbitMQEventBus> _logger;

    public RabbitMQEventBus(
        string hostname,
        string connectionName,
        ILogger<RabbitMQEventBus> logger)
    {
        var factory = new ConnectionFactory
        {
            HostName = hostname,
            ClientProvidedName = connectionName
        };

        _connection = factory.CreateConnection();

        _channel = _connection.CreateModel();

        _logger = logger;
    }

    public async Task PublishAsync<T>(T message, string queueName)
    {
        _channel.QueueDeclare(queueName, true, false, false, null);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        _channel.BasicPublish(string.Empty, queueName, true, null, body);

        _logger.LogInformation("Message published to queue {QueueName} with message: {Message}", queueName, message);

        await Task.CompletedTask;
    }
}
