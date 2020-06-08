using UsersAdmin.Core.Model.User;
using UsersAdmin.Data.Repositories;

namespace UsersAdmin.Test.Unit.Repository.User
{
    public class UserRepositoryInsertTest : RepositoryBaseInsertTest<UserEntity, UserRepository>
    {
        public UserRepositoryInsertTest()
            : base(new UserRepositoryTest())
        {

        }
    }
}