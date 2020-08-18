using System.Collections.Generic;
using System.Threading.Tasks;
using Tatisoft.UsersAdmin.Core.Model.System;

namespace Tatisoft.UsersAdmin.Core.Repositories
{
    public interface ISystemRepository : IRepository<SystemEntity>
    {
        SystemEntity SelectIncludingUsers(string systemId);
    }
}
