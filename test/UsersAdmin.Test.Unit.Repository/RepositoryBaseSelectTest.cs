using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tatisoft.UsersAdmin.Core.Repositories;
using Tatisoft.UsersAdmin.Data;
using Xunit;

namespace Tatisoft.UsersAdmin.Test.Unit.Repository
{
    using static Testing;

    public abstract class RepositoryBaseSelectTest<TEntity, TRepository>
        where TEntity : class, IIds
        where TRepository : IRepository<TEntity>
    {
        protected readonly IBaseRepositoryTest<TEntity, TRepository> _baseRepository;

        public RepositoryBaseSelectTest(IBaseRepositoryTest<TEntity, TRepository> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        [Fact]
        public async void SelectById_ObtainOne()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                TRepository repo = _baseRepository.GetNewRepository(context);
                TEntity entity = _baseRepository.GetNewValidEntity();
                await context.Set<TEntity>().AddAsync(entity);
                await context.SaveChangesAsync();

                var entityById = await repo.SelectByIdAsync(entity.GetIds);

                Assert.NotNull(entityById);
                _baseRepository.AssertAllProperties(entity, context.Set<TEntity>().First());
            }
        }

        [Fact]
        public async void SelectById_Null_IsNull()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                TRepository repo = _baseRepository.GetNewRepository(context);

                var entityById = await repo.SelectByIdAsync(null);

                Assert.Null(entityById);
            }
        }

        [Fact]
        public async void SelectAll_IsEmpty()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                TRepository repo = _baseRepository.GetNewRepository(context);

                var entities = await repo.SelectAllAsync();

                Assert.Empty(entities);
            }
        }

        [Fact]
        public async void SelectAll_ObtainOne()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                TRepository repo = _baseRepository.GetNewRepository(context);
                TEntity entity = _baseRepository.GetNewValidEntity();
                await context.Set<TEntity>().AddAsync(entity);
                await context.SaveChangesAsync();

                var entities = await repo.SelectAllAsync();

                Assert.NotEmpty(entities);
                Assert.Single(entities);
                _baseRepository.AssertAllProperties(entity, entities.First());
            }
        }

        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 0)]
        public async void SelectByFilter_ValidateBasicPredicates(bool expressionFilterResult, int expectedCant)
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                TRepository repo = _baseRepository.GetNewRepository(context);
                TEntity entity = _baseRepository.GetNewValidEntity();
                await context.Set<TEntity>().AddAsync(entity);
                await context.SaveChangesAsync();

                Expression<Func<TEntity, bool>> expressionFunctionFilter = (s) => expressionFilterResult;

                var entities = repo.SelectByFilter(expressionFunctionFilter);

                Assert.NotNull(entities);
                Assert.Equal(expectedCant, entities.Count());
            }
        }
    }
}