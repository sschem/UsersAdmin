using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tatisoft.UsersAdmin.Core.Services
{
    public interface IService<TDto, TEntity>
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        ValueTask<TDto> GetByIdAsync(params object[] idValues);
        Task<TDto> AddAsync(TDto dto);        
        Task Modify(TDto dto, params object[] idValues);
        Task ModifyConnectedEntity(TEntity entity);
        Task Remove(params object[] idValues);
    }
}