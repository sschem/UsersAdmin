using System.Collections.Generic;
using UsersAdmin.Core.Model.User;

namespace UsersAdmin.Core.Repositories
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        IEnumerable<UserEntity> SelectItemsByNameFilter(string nameFilter);
    }
}
