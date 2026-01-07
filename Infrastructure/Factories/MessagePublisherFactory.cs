using Application.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Factories
{
    public class MessagePublisherFactory : IMessagePublisherFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public MessagePublisherFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IMessagePublisher GetPublisher(string publisherType)
        {
            return publisherType switch
            {
                "RabbitMQ" => _serviceProvider.GetRequiredService<RabbitMQPublisher>(),
                "ServiceBus" => _serviceProvider.GetRequiredService<ServiceBusPublisher>(),
                _ => throw new ArgumentException($"Unknown publisher type: {publisherType}")
            };
        }
    }
}