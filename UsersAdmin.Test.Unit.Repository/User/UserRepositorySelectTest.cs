using System.Linq;
using UsersAdmin.Core.Model.User;
using UsersAdmin.Data;
using UsersAdmin.Data.Repositories;
using Xunit;

namespace UsersAdmin.Test.Unit.Repository.User
{
    using static Testing;
    public class UserRepositorySelectTest : RepositoryBaseSelectTest<UserEntity, UserRepository>
    {
        public UserRepositorySelectTest()
            : base(new UserRepositoryTest())
        {

        }

        [Fact]
        public async void SelectItemsByNameFilter_ValidateOk()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                UserRepository repo = _baseRepository.GetNewRepository(context);
                UserEntity userEntity = _baseRepository.GetNewValidEntity();
                await context.Users.AddAsync(userEntity);
                await context.SaveChangesAsync();

                var selectedEntities = repo.SelectItemsByNameFilter(userEntity.Name);

                Assert.NotNull(selectedEntities);
                Assert.Single(selectedEntities);
                _baseRepository.AssertAllProperties(userEntity, selectedEntities.First());
            }
        }

        [Theory]
        [InlineData("x")]
        [InlineData(null)]
        public void SelectItemsByNameFilter_NonExistent_returnEmptyList(string id)
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                UserRepository repo = _baseRepository.GetNewRepository(context);

                var entityWithUsers = repo.SelectItemsByNameFilter(id);

                Assert.Empty(entityWithUsers);
            }
        }
    }
}
