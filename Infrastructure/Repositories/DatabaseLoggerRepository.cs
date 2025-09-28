using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class DatabaseLoggerRepository : IDatabaseLoggerRepository
    {
        private readonly OrdersDbContext _context;

        // Injeta o DbContext via construtor
        public DatabaseLoggerRepository(OrdersDbContext context)
        {
            _context = context;
        }

        public async Task LogTraceAsync(Trace log)
        {
            await _context.Traces.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task LogRequestAsync(RequestLog log)
        {
            await _context.RequestLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRequestLogAsync(RequestLog log)
        {
            _context.RequestLogs.Update(log);
            await _context.SaveChangesAsync();
        }
    }
}
