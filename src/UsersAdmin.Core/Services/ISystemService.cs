using System.Collections.Generic;
using System.Threading.Tasks;
using Tatisoft.UsersAdmin.Core.Model.System;

namespace Tatisoft.UsersAdmin.Core.Services
{
    public interface ISystemService : IService<SystemDto, SystemEntity>
    {
        SystemDto GetWithUsers(string systemId);

        Task<IEnumerable<SystemItemDto>> GetAllItemsAsync();
    }
}