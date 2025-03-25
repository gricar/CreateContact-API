using CreateContact.API.IntegrationTests.Infrastructure;
using CreateContact.Application.Contact.Commands.Create;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit.Abstractions;

namespace CreateContact.API.IntegrationTests
{
    public class CreateContact : IClassFixture<ApiFactory>
    {
        private readonly HttpClient _apiClient;
        private readonly RabbitMqConsumer _rabbitMqConsumer;
        private readonly ITestOutputHelper _output;

        public CreateContact(ApiFactory factory, ITestOutputHelper output)
        {
            _apiClient = factory.CreateClient();
            _rabbitMqConsumer = factory.RabbitMqConsumer;
            _output = output;
        }

        [Fact]
        public async Task Should_publish_event_when_contact_created()
        {
            // Arrange
            var contact = new CreateContactCommand("Ayrton", 11, "123456789");
            var queueName = "contact-created";
            _rabbitMqConsumer.BindQueue(queueName);

            // Act
            _output.WriteLine("Enviando requisição para /api/contacts...");
            var response = await _apiClient.PostAsJsonAsync("/api/Contacts", contact);

            var result = await _rabbitMqConsumer.TryToConsumeAsync(queueName, TimeSpan.FromSeconds(5));

            var responseText = await response.Content.ReadAsStringAsync();
            _output.WriteLine($"responseText {responseText}");
            var msg = JsonSerializer.Deserialize<CreateContactCommandResponse>(responseText);

            // Assert
            result.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.Accepted);
            msg.message.Should().Be("Contact creation request accepted and is being processed.");
        }
    }
}