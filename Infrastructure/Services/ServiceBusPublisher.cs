using Application.Interfaces;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class ServiceBusPublisher : IServiceBusPublisher
    {
        private readonly ServiceBusClient _serviceBusClient;

        public ServiceBusPublisher(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("FCGServiceBusConnection");
            _serviceBusClient = new ServiceBusClient(connectionString);
        }

        public async Task PublishMessageAsync(string topicName, object message, IDictionary<string, object>? customProperties = null)
        {
            var sender = _serviceBusClient.CreateSender(topicName);
            var serviceBusMessage = new ServiceBusMessage(System.Text.Json.JsonSerializer.Serialize(message))
            {
                ContentType = "application/json"
            };

            // Adding custom properties if provided
            if (customProperties != null)
            {
                foreach (var kv in customProperties)
                    serviceBusMessage.ApplicationProperties[kv.Key] = kv.Value;
            }

            await sender.SendMessageAsync(serviceBusMessage);
        }
    }
}