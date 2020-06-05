using System.Linq;
using UsersAdmin.Core.Model.User;
using UsersAdmin.Data;
using UsersAdmin.Data.Repositories;
using Xunit;

namespace UsersAdmin.Test.Unit.Repository.User
{
    using static Testing;
    public class UserRepositorySelectTest
    {
        [Fact]
        public async void UserRepository_SelectItemsByNameFilter_ValidateOk()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                UserRepository repo = new UserRepository(context);
                UserEntity userEntity = GetValidUserEntity();
                await context.Users.AddAsync(userEntity);
                await context.SaveChangesAsync();

                var selectedEntities = repo.SelectItemsByNameFilter(userEntity.Name);

                Assert.NotNull(selectedEntities);
                Assert.Single(selectedEntities);
                Assert.Equal(userEntity.Id, selectedEntities.First().Id);
                Assert.Equal(userEntity.Name, selectedEntities.First().Name);
                Assert.Equal(userEntity.Description, selectedEntities.First().Description);
                Assert.Equal(userEntity.Email, selectedEntities.First().Email);
                Assert.Equal(userEntity.Pass, selectedEntities.First().Pass);
            }
        }

        [Theory]
        [InlineData("x")]
        [InlineData(null)]
        public void UserRepository_SelectItemsByNameFilter_NonExistent_returnEmptyList(string id)
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                UserRepository repo = new UserRepository(context);

                var entityWithUsers = repo.SelectItemsByNameFilter(id);

                Assert.Empty(entityWithUsers);
            }
        }
    }
}
