using System.Threading.Tasks;
using UsersAdmin.Core;
using UsersAdmin.Core.Repositories;
using UsersAdmin.Data.Repositories;

namespace UsersAdmin.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthDbContext _context;
        private SystemRepository _systemRepository;

        public UnitOfWork(AuthDbContext context)
        {
            this._context = context;
        }

        public ISystemRepository Systems => _systemRepository = _systemRepository ?? new SystemRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}