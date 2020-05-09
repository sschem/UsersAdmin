using System.Collections.Generic;
using System.Threading.Tasks;

namespace UsersAdmin.Core.Services
{
    public interface IService<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(params object[] idValues);
        Task<TEntity> AddAsync(TEntity entity);        
        Task Modify(TEntity entity, params object[] idValues);
        Task ModifyConnectedEntity(TEntity entity);
        Task Remove(params object[] idValues);
    }
}