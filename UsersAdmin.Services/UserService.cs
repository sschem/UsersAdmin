using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersAdmin.Core.Model.User;
using UsersAdmin.Core.Repositories;
using UsersAdmin.Core.Services;

namespace UsersAdmin.Services
{
    public class UserService : ServiceBase<UserDto, UserEntity, IUserRepository>, IUserService
    {
        protected override IUserRepository Repository => _unitOfWork.Users;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IAppCache cache) 
            : base(unitOfWork, mapper, cache) { }

        protected override void MapPropertiesForUpdate(UserEntity outdatedEntity, UserEntity newEntity)
        {
            outdatedEntity.Name = newEntity.Name;
            outdatedEntity.Description = newEntity.Description;
            outdatedEntity.Email = newEntity.Email;
            outdatedEntity.Pass = newEntity.Pass;
        }

        public async Task<IEnumerable<UserItemDto>> GetAllItemsAsync()
        {
            var entities = await this.GetAllEntitiesAsync();
            var UserItems = _mapper.Map<IEnumerable<UserItemDto>>(entities);
            return UserItems;
        }

        public async Task<IEnumerable<UserItemDto>> GetItemsByNameFilter(string nameFilter)
        {
            var entities = await this.GetAllEntitiesAsync();
            
            var filterEntities = entities.Where(u => !string.IsNullOrEmpty(nameFilter) &&
                (u.Name.ToUpper().Contains(nameFilter.ToUpper())));

            var UserItems = _mapper.Map<IEnumerable<UserItemDto>>(filterEntities);
            return UserItems;
        }
    }
}