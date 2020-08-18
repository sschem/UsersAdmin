using Tatisoft.UsersAdmin.Core.Model.User;
using Tatisoft.UsersAdmin.Data;
using Tatisoft.UsersAdmin.Data.Repositories;
using Xunit;

namespace Tatisoft.UsersAdmin.Test.Unit.Repository.User
{
    using static Testing;

    public class UserRepositoryTest : IBaseRepositoryTest<UserEntity, UserRepository>
    {
        public void AssertAllProperties(UserEntity expected, UserEntity obtained)
        {
            Assert.Equal(expected.Id, obtained.Id);
            Assert.Equal(expected.Name, obtained.Name);
            Assert.Equal(expected.Description, obtained.Description);
            Assert.Equal(expected.Email, obtained.Email);
            Assert.Equal(expected.Pass, obtained.Pass);
        }

        public void ChangeIdToNonExistent(ref UserEntity entity)
        {
            entity.Id = "_x";
        }

        public void ChangeIdToNull(ref UserEntity entity)
        {
            entity.Id = null;
        }

        public void ChangeNotIdProperties(ref UserEntity entity)
        {
            entity.Name = "AnotherName";
            entity.Description = "AnotherDescription";
            entity.Email = "AnotherEmail";
            entity.Pass = "AnotherPass";
        }

        public UserRepository GetNewRepository(AuthDbContext context)
        {
            return new UserRepository(context);
        }

        public UserEntity GetNewValidEntity()
        {
            return GetValidUserEntity();
        }
    }
}
