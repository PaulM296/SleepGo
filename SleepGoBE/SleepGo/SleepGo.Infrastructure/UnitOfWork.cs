using SleepGo.App.Interfaces;

namespace SleepGo.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SleepGoDbContext _context;
        public UnitOfWork(SleepGoDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
