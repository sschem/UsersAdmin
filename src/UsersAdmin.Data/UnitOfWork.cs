using System.Threading.Tasks;
using Tatisoft.UsersAdmin.Core.Repositories;
using Tatisoft.UsersAdmin.Data.Repositories;

namespace Tatisoft.UsersAdmin.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthDbContext _context;
        private SystemRepository _systemRepository;
        private UserRepository _userRepository;

        public UnitOfWork(AuthDbContext context)
        {
            this._context = context;
        }

        public ISystemRepository Systems => _systemRepository ??= new SystemRepository(_context);

        public IUserRepository Users => _userRepository ??= new UserRepository(_context);

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