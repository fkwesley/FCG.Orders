using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class RabbitMQPublisher : IMessagePublisher, IDisposable
    {
        private readonly IConnection _connection;

        public RabbitMQPublisher(IConfiguration configuration)
        {
            var connectionString =
                configuration.GetConnectionString("FCGRabbitMQConnection")
                ?? throw new ArgumentNullException("RabbitMQ connection string not found.");

            var factory = new ConnectionFactory
            {
                Uri = new Uri(connectionString)
            };

            // conexão assíncrona no startup (OK)
            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        }

        public async Task PublishMessageAsync(string queueName, object message, IDictionary<string, object>? customProperties = null)
        {
            await using var channel = await _connection.CreateChannelAsync();

            // garante que a fila existe
            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            var properties = new BasicProperties
            {
                ContentType = "application/json",
                DeliveryMode = DeliveryModes.Persistent,
                Headers = customProperties
            };

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: queueName,
                mandatory: false,
                basicProperties: properties,
                body: body);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
