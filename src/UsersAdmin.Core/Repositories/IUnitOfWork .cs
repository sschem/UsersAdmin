using System;
using System.Threading.Tasks;

namespace Tatisoft.UsersAdmin.Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ISystemRepository Systems { get; }

        IUserRepository Users { get; }

        Task<int> CommitAsync();
    }
}