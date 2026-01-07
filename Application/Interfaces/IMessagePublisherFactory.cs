namespace Application.Interfaces
{
    public interface IMessagePublisherFactory
    {
        IMessagePublisher GetPublisher(string publisherType);
    }
}