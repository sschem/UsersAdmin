using System.Collections.Generic;
using System.Threading.Tasks;
using UsersAdmin.Core.Model.User;

namespace UsersAdmin.Core.Services
{
    public interface IUserService : IService<UserDto, UserEntity>
    {
        Task<IEnumerable<UserItemDto>> GetAllItemsAsync();
        
        Task<IEnumerable<UserItemDto>> GetItemsByNameFilter(string nameFilter);

        Task<UserLoggedDto> LoginAsAdminAsync(UserLoginDto user);

        Task<UserLoggedDto> LoginInSystemAsync(UserLoginDto user, string systemId);
    }
}