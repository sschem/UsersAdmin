using System.Collections.Generic;
using System.Threading.Tasks;
using UsersAdmin.Core.Models;

namespace UsersAdmin.Core.Services
{
    public interface ISystemService : IService<SystemEntity>
    {
        Task<IEnumerable<SystemEntity>> GetByUser(string userId);
    }
}