using System.Collections.Generic;
using System.Threading.Tasks;
using Tatisoft.UsersAdmin.Core.Model.User;

namespace Tatisoft.UsersAdmin.Core.Services
{
    public interface IUserService : IService<UserDto, UserEntity>
    {
        Task<IEnumerable<UserItemDto>> GetAllItemsAsync();
        
        Task<IEnumerable<UserItemDto>> GetItemsByNameFilter(string nameFilter);

        Task<UserLoggedDto> LoginAsAdminAsync(UserLoginDto user);

        Task<UserLoggedDto> LoginInSystemAsync(UserLoginDto user, string systemId);

        Task<UserDto> GetBySystemAsync(string userId, string systemId);

        Task AssociateUserSystemAsync(string userId, string systemId);

        Task UnassociateUserSystemAsync(string userId, string systemId);
    }
}