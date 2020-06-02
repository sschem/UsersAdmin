using System.Collections.Generic;
using System.Threading.Tasks;
using UsersAdmin.Core.Model.System;

namespace UsersAdmin.Core.Services
{
    public interface ISystemService : IService<SystemDto, SystemEntity>
    {
        Task<IEnumerable<SystemDto>> GetByUserAsync(string userId);

        Task<IEnumerable<SystemItemDto>> GetAllItemsAsync();
    }
}