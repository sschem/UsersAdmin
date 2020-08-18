using Tatisoft.UsersAdmin.Core.Model.User;
using Tatisoft.UsersAdmin.Data.Repositories;

namespace Tatisoft.UsersAdmin.Test.Unit.Repository.User
{
    public class UserRepositoryInsertTest : RepositoryBaseInsertTest<UserEntity, UserRepository>
    {
        public UserRepositoryInsertTest()
            : base(new UserRepositoryTest())
        {

        }
    }
}