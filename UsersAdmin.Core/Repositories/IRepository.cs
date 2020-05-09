using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UsersAdmin.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        ValueTask<TEntity> SelectByIdAsync(params object[] keyValues);
        IEnumerable<TEntity> SelectByFilter(Expression<Func<TEntity, bool>> predicate);        
        Task<IEnumerable<TEntity>> SelectAllAsync();        
        Task InsertAsync(TEntity entity);
        Task InsertRangeAsync(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
    }
}
