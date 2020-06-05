using System;
using System.Linq;
using System.Linq.Expressions;
using UsersAdmin.Core.Model.System;
using UsersAdmin.Core.Model.User;
using UsersAdmin.Data;
using UsersAdmin.Data.Repositories;
using Xunit;

namespace UsersAdmin.Test.Unit.Repository
{
    using static Testing;
    public class SystemRepositorySelectTest
    {
        [Fact]
        public async void SystemRepository_SelectById_ObtainOne()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                SystemRepository repo = new SystemRepository(context);
                SystemEntity entity = GetValidSystemEntity();
                await context.Systems.AddAsync(entity);
                await context.SaveChangesAsync();

                var entityById = await repo.SelectByIdAsync(entity.GetIds);

                Assert.NotNull(entityById);
                Assert.Equal(entity.Id, context.Systems.First().Id);
                Assert.Equal(entity.Name, context.Systems.First().Name);
                Assert.Equal(entity.Description, context.Systems.First().Description);
            }
        }

        [Fact]
        public async void SystemRepository_SelectById_Null_IsNull()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                SystemRepository repo = new SystemRepository(context);

                var entityById = await repo.SelectByIdAsync(null);

                Assert.Null(entityById);
            }
        }

        [Fact]
        public async void SystemRepository_SelectAll_IsEmpty()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                SystemRepository repo = new SystemRepository(context);

                var entities = await repo.SelectAllAsync();

                Assert.Empty(entities);
            }
        }

        [Fact]
        public async void SystemRepository_SelectAll_ObtainOne()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                SystemRepository repo = new SystemRepository(context);
                SystemEntity entity = GetValidSystemEntity();
                await context.Systems.AddAsync(entity);
                await context.SaveChangesAsync();

                var entities = await repo.SelectAllAsync();

                Assert.NotEmpty(entities);
                Assert.Single(entities);
                Assert.Equal(entity.Id, entities.First().Id);
                Assert.Equal(entity.Name, entities.First().Name);
                Assert.Equal(entity.Description, entities.First().Description);
            }
        }

        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 0)]
        public async void SystemRepository_SelectByFilter_ValidateBasicPredicates(bool expressionFilterResult, int expectedCant)
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                SystemRepository repo = new SystemRepository(context);
                SystemEntity entity = GetValidSystemEntity();
                await context.Systems.AddAsync(entity);
                await context.SaveChangesAsync();

                Expression<Func<SystemEntity, bool>> expressionFunctionFilter = (s) => expressionFilterResult;

                var entities = repo.SelectByFilter(expressionFunctionFilter);

                Assert.NotNull(entities);
                Assert.Equal(expectedCant, entities.Count());
            }
        }

        [Fact]
        public async void SystemRepository_SelectIncludingUsers_ObtainOneWithUser()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                SystemRepository repo = new SystemRepository(context);
                SystemEntity systemEntity = GetValidSystemEntity();
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
                Assert.Equal(systemEntity.Id, entityWithUsers.Id);
                Assert.Equal(systemEntity.Name, entityWithUsers.Name);
                Assert.Equal(systemEntity.Description, entityWithUsers.Description);                
                Assert.Single(entityWithUsers.UserSystemLst);
                Assert.Equal(systemEntity.Id, entityWithUsers.UserSystemLst.First().SystemId);
                Assert.Equal(userEntity.Id, entityWithUsers.UserSystemLst.First().UserId);                
                Assert.Equal(userEntity.Name, entityWithUsers.UserSystemLst.First().User.Name);
            }
        }

        [Fact]
        public async void SystemRepository_SelectIncludingUsers_SystemWithoutUsers_then_UserListIsEmpty()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                SystemRepository repo = new SystemRepository(context);
                SystemEntity systemEntity = GetValidSystemEntity();
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
