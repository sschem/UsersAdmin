using System.Linq;
using UsersAdmin.Core.Model.System;
using UsersAdmin.Core.Model.User;
using UsersAdmin.Data;
using UsersAdmin.Data.Repositories;
using Xunit;

namespace UsersAdmin.Test.Unit.Repository.System
{
    using static Testing;
    public class SystemRepositorySelectTest : RepositoryBaseSelectTest<SystemEntity, SystemRepository>
    {
        public SystemRepositorySelectTest()
            : base(new SystemRepositoryTest())
        {

        }

        [Fact]
        public async void SystemRepository_SelectIncludingUsers_ObtainOneWithUser()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                SystemRepository repo = _baseRepository.GetNewRepository(context);
                SystemEntity systemEntity = _baseRepository.GetNewValidEntity();
                await context.Systems.AddAsync(systemEntity);
                UserEntity userEntity = GetValidUserEntity();
                await context.Users.AddAsync(userEntity);
                UserSystemEntity userSysEntity = new UserSystemEntity()
                {
                    SystemId = systemEntity.Id,
                    UserId = userEntity.Id
                };
                await context.UsersSystems.AddAsync(userSysEntity);
                await context.SaveChangesAsync();

                var entityWithUsers = repo.SelectIncludingUsers(systemEntity.Id);

                Assert.NotNull(entityWithUsers);
                _baseRepository.AssertAllProperties(systemEntity, entityWithUsers);
                Assert.Single(entityWithUsers.UserSystemLst);                
                _baseRepository.AssertAllProperties(systemEntity, entityWithUsers.UserSystemLst.First().System);
                Assert.Equal(userEntity.Id, entityWithUsers.UserSystemLst.First().UserId);
                Assert.Equal(userEntity.Name, entityWithUsers.UserSystemLst.First().User.Name);
            }
        }

        [Fact]
        public async void SystemRepository_SelectIncludingUsers_SystemWithoutUsers_then_UserListIsEmpty()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                SystemRepository repo = _baseRepository.GetNewRepository(context);
                SystemEntity systemEntity = _baseRepository.GetNewValidEntity();
                await context.Systems.AddAsync(systemEntity);
                await context.SaveChangesAsync();

                var entityWithUsers = repo.SelectIncludingUsers(systemEntity.Id);

                Assert.NotNull(entityWithUsers);
                Assert.Equal(systemEntity.Id, entityWithUsers.Id);
                Assert.NotNull(entityWithUsers.UserSystemLst);
                Assert.Empty(entityWithUsers.UserSystemLst);
            }
        }

        [Theory]
        [InlineData("x")]
        [InlineData(null)]
        public void SystemRepository_SelectIncludingUsers_NonExistent_returnNull(string id)
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                SystemRepository repo = new SystemRepository(context);

                var entityWithUsers = repo.SelectIncludingUsers(id);

                Assert.Null(entityWithUsers);
            }
        }
    }
}
