using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CreateContact.API.IntegrationTests.Infrastructure;

public class RabbitMqConsumer
{
    private readonly ConnectionFactory _factory;

    public RabbitMqConsumer(string connectionString)
    {
        _factory = new ConnectionFactory()
        {
            Uri = new Uri(connectionString)
        };
    }

    public void BindQueue(string queueName)
    {
        using var connection = _factory.CreateConnection();

        using var channel = connection.CreateModel();

        channel.QueueDeclare(queueName, true, false, false, null);
    }

    public async Task<bool> TryToConsumeAsync(string queueName, TimeSpan timeout)
    {
        var messageReceived = new TaskCompletionSource<bool>();

        using var connection = _factory.CreateConnection();

        using var channel = connection.CreateModel();

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (sender, eventArgs) =>
        {
            messageReceived.SetResult(true);
        };

        channel.BasicConsume(queueName, true, consumer);

        var timeoutTask = Task.Delay(timeout);

        var completedTask = await Task.WhenAny(messageReceived.Task, timeoutTask);

        return completedTask == messageReceived.Task;
    }
}
