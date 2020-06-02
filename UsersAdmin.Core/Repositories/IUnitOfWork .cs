using System;
using System.Threading.Tasks;

namespace UsersAdmin.Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ISystemRepository Systems { get; }
        Task<int> CommitAsync();
    }
}