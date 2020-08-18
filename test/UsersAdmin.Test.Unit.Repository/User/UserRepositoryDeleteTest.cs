using Tatisoft.UsersAdmin.Core.Model.User;
using Tatisoft.UsersAdmin.Data.Repositories;

namespace Tatisoft.UsersAdmin.Test.Unit.Repository.User
{
    public class UserRepositoryDeleteTest : RepositoryBaseDeleteTest<UserEntity, UserRepository>
    {
        public UserRepositoryDeleteTest()
            : base(new UserRepositoryTest())
        {

        }
    }
}