using UsersAdmin.Core.Model.User;
using UsersAdmin.Data.Repositories;

namespace UsersAdmin.Test.Unit.Repository.User
{
    public class UserRepositoryDeleteTest : RepositoryBaseDeleteTest<UserEntity, UserRepository>
    {
        public UserRepositoryDeleteTest()
            : base(new UserRepositoryTest())
        {

        }
    }
}