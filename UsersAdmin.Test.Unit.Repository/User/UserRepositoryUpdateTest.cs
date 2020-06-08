using UsersAdmin.Core.Model.User;
using UsersAdmin.Data.Repositories;

namespace UsersAdmin.Test.Unit.Repository.User
{
    public class UserRepositoryUpdateTest : RepositoryBaseUpdateTest<UserEntity, UserRepository>
    {
        public UserRepositoryUpdateTest()
            : base(new UserRepositoryTest())
        {

        }
    }
}