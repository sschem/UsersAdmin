﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Tatisoft.UsersAdmin.Core.Repositories;
using Tatisoft.UsersAdmin.Data;
using Xunit;

namespace Tatisoft.UsersAdmin.Test.Unit.Repository
{
    using static Testing;

    public abstract class RepositoryBaseDeleteTest<TEntity, TRepository>
        where TEntity : class, IIds
        where TRepository : IRepository<TEntity>
    {
        protected readonly IBaseRepositoryTest<TEntity, TRepository> _baseRepository;

        public RepositoryBaseDeleteTest(IBaseRepositoryTest<TEntity, TRepository> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        [Fact]
        public async void Delete_ValidateOk()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                TRepository repo = _baseRepository.GetNewRepository(context);
                TEntity entity = _baseRepository.GetNewValidEntity();
                await context.Set<TEntity>().AddAsync(entity);
                await context.SaveChangesAsync();

                repo.Delete(entity);
                await context.SaveChangesAsync();

                Assert.Empty(context.Systems);
            }
        }

        [Fact]
        public async void Delete_NonExistent_Id_ThrowException()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                TRepository repo = _baseRepository.GetNewRepository(context);
                TEntity entity = _baseRepository.GetNewValidEntity();
                _baseRepository.ChangeIdToNonExistent(ref entity);

                Func<Task> deleteAction = async () =>
                {
                    repo.Delete(entity);
                    await context.SaveChangesAsync();
                };

                await Assert.ThrowsAsync<DbUpdateConcurrencyException>(deleteAction);
            }
        }

        [Fact]
        public async void Delete_Null_Entity_ThrowException()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                TRepository repo = _baseRepository.GetNewRepository(context);
                TEntity entity = null;

                Func<Task> deleteAction = async () =>
                {
                    repo.Delete(entity);
                    await context.SaveChangesAsync();
                };

                await Assert.ThrowsAsync<ArgumentNullException>(deleteAction);
            }
        }

        [Fact]
        public async void Delete_Null_Id_ThrowException()
        {
            using (var context = new AuthDbContext(CreateNewContextOptions()))
            {
                TRepository repo = _baseRepository.GetNewRepository(context);
                TEntity entity = _baseRepository.GetNewValidEntity();
                _baseRepository.ChangeIdToNull(ref entity);

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
