using System;
using System.Threading.Tasks;
using UsersAdmin.Core.Repositories;

namespace UsersAdmin.Core
{
    public interface IUnitOfWork : IDisposable
    {
        ISystemRepository Systems { get; }
        Task<int> CommitAsync();
    }
}