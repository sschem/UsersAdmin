using System;
using System.Linq;
using System.Threading.Tasks;
using UsersAdmin.Core.Repositories;
using UsersAdmin.Data;
using Xunit;

namespace UsersAdmin.Test.Unit.Repository
{
    using static Testing;

    public abstract class RepositoryBaseInsertTest<TEntity, TRepository>
        where TEntity : class, IIds
        where TRepository : IRepository<TEntity>
    {
        protected readonly IBaseRepositoryTest<TEntity, TRepository> _baseRepository;

        public RepositoryBaseInsertTest(IBaseRepositoryTest<TEntity, TRepository> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        [Fact]
        public async void Repository_Insert_ValidateOk()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                TRepository repo = _baseRepository.GetNewRepository(context);
                TEntity entity = _baseRepository.GetNewValidEntity();

                await repo.InsertAsync(entity);
                await context.SaveChangesAsync();

                Assert.Equal(1, context.Set<TEntity>().Count());
                _baseRepository.AssertAllProperties(entity, context.Set<TEntity>().First());
            }
        }

        [Fact]
        public async void Repository_Insert_Existent_Id_ThrowException()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                TRepository repo = _baseRepository.GetNewRepository(context);
                TEntity entity = _baseRepository.GetNewValidEntity();
                await context.Set<TEntity>().AddAsync(entity);
                await context.SaveChangesAsync();
                _baseRepository.ChangeNotIdProperties(ref entity);

                Func<Task> insertFunction = async () =>
                {
                    await repo.InsertAsync(entity);
                    await context.SaveChangesAsync();
                };

                await Assert.ThrowsAsync<ArgumentException>(insertFunction);
            }
        }

        [Fact]
        public async void Repository_Insert_Null_Entity_ThrowException()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                TRepository repo = _baseRepository.GetNewRepository(context);
                TEntity entity = null;

                Func<Task> insertFunction = async () =>
                {
                    await repo.InsertAsync(entity);
                    await context.SaveChangesAsync();
                };

                await Assert.ThrowsAsync<ArgumentNullException>(insertFunction);
            }
        }

        [Fact]
        public async void Repository_Insert_Null_Id_ThrowException()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                TRepository repo = _baseRepository.GetNewRepository(context);
                TEntity entity = _baseRepository.GetNewValidEntity();

                _baseRepository.ChangeIdToNull(ref entity);

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
