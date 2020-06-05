using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UsersAdmin.Core.Model.System;
using UsersAdmin.Data;
using UsersAdmin.Data.Repositories;
using Xunit;

namespace UsersAdmin.Test.Unit.Repository
{
    using static Testing;
    public class SystemRepositoryDeleteTest
    {
        [Fact]
        public async void SystemRepository_Delete_ValidateOk()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                SystemRepository repo = new SystemRepository(context);
                SystemEntity entity = GetValidSystemEntity();
                await context.Systems.AddAsync(entity);
                await context.SaveChangesAsync();

                repo.Delete(entity);
                await context.SaveChangesAsync();

                Assert.Empty(context.Systems);
            }
        }

        [Fact]
        public async void SystemRepository_Delete_NonExistent_Id_ThrowException()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                SystemRepository repo = new SystemRepository(context);
                SystemEntity entity = GetValidSystemEntity();
                entity.Id = "x";

                Func<Task> deleteAction = async () =>
                {
                    repo.Delete(entity);
                    await context.SaveChangesAsync();
                };

                await Assert.ThrowsAsync<DbUpdateConcurrencyException>(deleteAction);
            }
        }

        [Fact]
        public async void SystemRepository_Delete_Null_Entity_ThrowException()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                SystemRepository repo = new SystemRepository(context);
                SystemEntity entity = null;

                Func<Task> deleteAction = async() =>
                {
                    repo.Delete(entity);
                    await context.SaveChangesAsync();
                };

                await Assert.ThrowsAsync<ArgumentNullException>(deleteAction);
            }
        }

        [Fact]
        public async void SystemRepository_Delete_Null_Id_ThrowException()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                SystemRepository repo = new SystemRepository(context);
                SystemEntity entity = GetValidSystemEntity();
                entity.Id = null;

                Func<Task> deleteAction = async () =>
                {
                    repo.Delete(entity);
                    await context.SaveChangesAsync();
                };

                await Assert.ThrowsAsync<InvalidOperationException>(deleteAction);
            }
        }
    }
}
