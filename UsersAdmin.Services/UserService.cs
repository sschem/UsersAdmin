using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using UsersAdmin.Core.Model.User;
using UsersAdmin.Core.Repositories;
using UsersAdmin.Core.Services;

namespace UsersAdmin.Services
{
    public class UserService : ServiceBase<UserDto, UserEntity, IUserRepository>, IUserService
    {
        protected override IUserRepository Repository => _unitOfWork.Users;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

        protected override void MapPropertiesForUpdate(UserEntity outdatedEntity, UserEntity newEntity)
        {
            outdatedEntity.Name = newEntity.Name;
            outdatedEntity.Description = newEntity.Description;
            outdatedEntity.Email = newEntity.Email;
            outdatedEntity.Pass = newEntity.Pass;
        }

        public async Task<IEnumerable<UserItemDto>> GetAllItemsAsync()
        {
            var entities = await Repository.SelectAllAsync();
            var UserItems = _mapper.Map<IEnumerable<UserItemDto>>(entities);
            return UserItems;
        }

        public IEnumerable<UserItemDto> GetItemsByNameFilter(string nameFilter)
        {
            var entities = Repository.SelectItemsByNameFilter(nameFilter);
            var UserItems = _mapper.Map<IEnumerable<UserItemDto>>(entities);
            return UserItems;
        }
    }
}