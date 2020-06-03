using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UsersAdmin.Core.Repositories;

namespace UsersAdmin.Data.Repositories
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly AuthDbContext Context;
        
        public RepositoryBase(AuthDbContext context)
        {
            this.Context = context;
        }

        public ValueTask<TEntity> SelectByIdAsync(params object[] keyValues)
        {
            return Context.Set<TEntity>().FindAsync(keyValues);
        }

        public IEnumerable<TEntity> SelectByFilter(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public async Task<IEnumerable<TEntity>> SelectAllAsync()
        {
            return  await Context.Set<TEntity>().ToListAsync();
        }
        
        public async Task InsertAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }

        public async Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            await Context.Set<TEntity>().AddRangeAsync(entities);
        }

        public void Update(TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }
    }
}