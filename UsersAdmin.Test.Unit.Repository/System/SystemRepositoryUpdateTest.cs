using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UsersAdmin.Core.Model.System;
using UsersAdmin.Data;
using UsersAdmin.Data.Repositories;
using Xunit;

namespace UsersAdmin.Test.Unit.Repository
{
    using static Testing;
    public class SystemRepositoryUpdateTest
    {
        [Fact]
        public async void SystemRepository_Update_ValidateOk()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                SystemRepository repo = new SystemRepository(context);
                SystemEntity entity = GetValidSystemEntity();
                await context.Systems.AddAsync(entity);
                await context.SaveChangesAsync();
                entity.Name = "UpdatedName";
                entity.Description = "UpdatedDescription";

                repo.Update(entity);
                await context.SaveChangesAsync();

                Assert.Equal(entity.Id, context.Systems.First().Id);
                Assert.Equal(entity.Name, context.Systems.First().Name);
                Assert.Equal(entity.Description, context.Systems.First().Description);
            }
        }

        [Fact]
        public async void SystemRepository_Update_NonExistent_Id_ThrowException()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                SystemRepository repo = new SystemRepository(context);
                SystemEntity entity = GetValidSystemEntity();
                entity.Id = "x";

                Func<Task> updateAction = async () =>
                {
                    repo.Update(entity);
                    await context.SaveChangesAsync();
                };

                await Assert.ThrowsAsync<DbUpdateConcurrencyException>(updateAction);
            }
        }

        [Fact]
        public async void SystemRepository_Update_Null_Entity_ThrowException()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                SystemRepository repo = new SystemRepository(context);
                SystemEntity entity = null;

                Func<Task> updateAction = async() =>
                {
                    repo.Update(entity);
                    await context.SaveChangesAsync();
                };

                await Assert.ThrowsAsync<ArgumentNullException>(updateAction);
            }
        }

        [Fact]
        public async void SystemRepository_Update_Null_Id_ThrowException()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                SystemRepository repo = new SystemRepository(context);
                SystemEntity entity = GetValidSystemEntity();
                entity.Id = null;

                Func<Task> updateAction = async () =>
                {
                    repo.Update(entity);
                    await context.SaveChangesAsync();
                };

                await Assert.ThrowsAsync<InvalidOperationException>(updateAction);
            }
        }
    }
}
