using System.Collections.Generic;
using Tatisoft.UsersAdmin.Core.Model.User;

namespace Tatisoft.UsersAdmin.Core.Repositories
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        IEnumerable<UserEntity> SelectItemsByNameFilter(string nameFilter);

        UserEntity SelectIncludingSystems(string userId);
    }
}
