using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UsersAdmin.Core.Repositories;
using UsersAdmin.Data;
using Xunit;

namespace UsersAdmin.Test.Unit.Repository
{
    using static Testing;

    public abstract class RepositoryBaseUpdateTest<TEntity, TRepository>
        where TEntity : class, IIds
        where TRepository : IRepository<TEntity>
    {
        protected readonly IBaseRepositoryTest<TEntity, TRepository> _baseRepository;

        public RepositoryBaseUpdateTest(IBaseRepositoryTest<TEntity, TRepository> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        [Fact]
        public async void Update_ValidateOk()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                TRepository repo = _baseRepository.GetNewRepository(context);
                TEntity entity = _baseRepository.GetNewValidEntity();
                await context.Set<TEntity>().AddAsync(entity);
                await context.SaveChangesAsync();
                _baseRepository.ChangeNotIdProperties(ref entity);

                repo.Update(entity);
                await context.SaveChangesAsync();

                _baseRepository.AssertAllProperties(entity, context.Set<TEntity>().First());
            }
        }

        [Fact]
        public async void Update_NonExistent_Id_ThrowException()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                TRepository repo = _baseRepository.GetNewRepository(context);
                TEntity entity = _baseRepository.GetNewValidEntity();
                _baseRepository.ChangeIdToNonExistent(ref entity);

                Func<Task> updateAction = async () =>
                {
                    repo.Update(entity);
                    await context.SaveChangesAsync();
                };

                await Assert.ThrowsAsync<DbUpdateConcurrencyException>(updateAction);
            }
        }

        [Fact]
        public async void Update_Null_Entity_ThrowException()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                TRepository repo = _baseRepository.GetNewRepository(context);
                TEntity entity = null;

                Func<Task> updateAction = async () =>
                {
                    repo.Update(entity);
                    await context.SaveChangesAsync();
                };

                await Assert.ThrowsAsync<ArgumentNullException>(updateAction);
            }
        }

        [Fact]
        public async void Update_Null_Id_ThrowException()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                TRepository repo = _baseRepository.GetNewRepository(context);
                TEntity entity = _baseRepository.GetNewValidEntity();
                _baseRepository.ChangeIdToNull(ref entity);

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