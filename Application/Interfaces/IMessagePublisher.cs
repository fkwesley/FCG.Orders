namespace Application.Interfaces
{
    public interface IMessagePublisher : IDisposable
    {
        Task PublishMessageAsync(string topicOrQueueName, object message, IDictionary<string, object>? customProperties = null);
    }
}