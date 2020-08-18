using Tatisoft.UsersAdmin.Core.Model.User;
using Tatisoft.UsersAdmin.Data.Repositories;

namespace Tatisoft.UsersAdmin.Test.Unit.Repository.User
{
    public class UserRepositoryUpdateTest : RepositoryBaseUpdateTest<UserEntity, UserRepository>
    {
        public UserRepositoryUpdateTest()
            : base(new UserRepositoryTest())
        {

        }
    }
}