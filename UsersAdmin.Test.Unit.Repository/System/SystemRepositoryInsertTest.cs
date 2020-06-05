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
    public class SystemRepositoryInsertTest
    {
        [Fact]
        public async void SystemRepository_Insert_ValidateOk()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                SystemRepository repo = new SystemRepository(context);
                SystemEntity entity = GetValidSystemEntity();

                await repo.InsertAsync(entity);
                await context.SaveChangesAsync();

                Assert.Equal(1, context.Systems.Count());
                Assert.Equal(entity.Id, context.Systems.First().Id);
                Assert.Equal(entity.Name, context.Systems.First().Name);
                Assert.Equal(entity.Description, context.Systems.First().Description);
            }
        }

        [Fact]
        public async void SystemRepository_Insert_Existent_Id_ThrowException()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                SystemRepository repo = new SystemRepository(context);
                SystemEntity entity = GetValidSystemEntity();
                await context.Systems.AddAsync(entity);
                await context.SaveChangesAsync();
                entity.Name = "AnotherName";
                entity.Description = "AnotherDescription";

                Func<Task> insertFunction = async () =>
                {
                    await repo.InsertAsync(entity);
                    await context.SaveChangesAsync();
                };

                await Assert.ThrowsAsync<ArgumentException>(insertFunction);
            }
        }

        [Fact]
        public async void SystemRepository_Insert_Null_Entity_ThrowException()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                SystemRepository repo = new SystemRepository(context);
                SystemEntity entity = null;

                Func<Task> insertFunction = async () =>
                {
                    await repo.InsertAsync(entity);
                    await context.SaveChangesAsync();
                };

                await Assert.ThrowsAsync<ArgumentNullException>(insertFunction);
            }
        }

        [Fact]
        public async void SystemRepository_Insert_Null_Id_ThrowException()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                SystemRepository repo = new SystemRepository(context);
                SystemEntity entity = GetValidSystemEntity();
                entity.Id = null;
                
                Func<Task> insertFunction = async () =>
                {
                    await repo.InsertAsync(entity);
                    await context.SaveChangesAsync();
                };

                await Assert.ThrowsAsync<InvalidOperationException>(insertFunction);
            }
        }
    }
}
