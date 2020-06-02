using System.Collections.Generic;
using System.Threading.Tasks;
using UsersAdmin.Core.Model.System;

namespace UsersAdmin.Core.Repositories
{
    public interface ISystemRepository : IRepository<SystemEntity>
    {
        Task<IEnumerable<SystemEntity>> SelectByUser(string userId);
    }
}
