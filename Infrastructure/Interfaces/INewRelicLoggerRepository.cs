
namespace Infrastructure.Interfaces
{
    public interface INewRelicLoggerRepository
    {
        Task SendLogAsync(object logObject);
    }
}
