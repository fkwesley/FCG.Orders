using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface IDatabaseLoggerRepository
    {
        Task LogTraceAsync(Trace log);
        Task LogRequestAsync(RequestLog log);
        Task UpdateRequestLogAsync(RequestLog log);
    }
}
