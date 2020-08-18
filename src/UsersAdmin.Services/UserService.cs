using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tatisoft.UsersAdmin.Core.Exceptions;
using Tatisoft.UsersAdmin.Core.Model.User;
using Tatisoft.UsersAdmin.Core.Repositories;
using Tatisoft.UsersAdmin.Core.Security;
using Tatisoft.UsersAdmin.Core.Services;

namespace Tatisoft.UsersAdmin.Services
{
    public class UserService : ServiceBase<UserDto, UserEntity, IUserRepository>, IUserService
    {
        protected override IUserRepository Repository => _unitOfWork.Users;
        protected ITokenProvider _tokenProvider;

        public virtual string UserIncorrect { get { return "Datos de Usuario incorrectos!"; } }

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IAppCache cache, ITokenProvider tokenProvider)
            : base(unitOfWork, mapper, cache)
        {
            _tokenProvider = tokenProvider;
        }

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

        public async Task<UserLoggedDto> LoginAsAdminAsync(UserLoginDto user)
        {
            var entities = await this.GetAllEntitiesAsync();
            var validatedUser = entities
                .Where(u => user != null &&
                    u.Id == user.Id && u.Pass == user.Pass &&
                    u.IsAdmin)
                .FirstOrDefault();

            var userLogged = this.LoginUser(validatedUser);
            return userLogged;
        }

        public async Task<UserLoggedDto> LoginInSystemAsync(UserLoginDto user, string systemId)
        {
            var entities = await this.GetAllEntitiesAsync();
            var validatedUser = entities
                .Where(u => user != null &&
                    u.Id == user.Id && u.Pass == user.Pass &&
                    !string.IsNullOrWhiteSpace(systemId))
                .FirstOrDefault();
            
            if (validatedUser != null)
            {
                validatedUser = this.Repository.SelectIncludingSystems(validatedUser.Id);
            }

            var userLogged = this.LoginUser(validatedUser, systemId);
            return userLogged;
        }

        private UserLoggedDto LoginUser(UserEntity validatedUser, string systemId = null)
        {
            if (validatedUser == null)
            {
                throw new WarningException(this.UserIncorrect);
            }
            else
            {
                var userLogged = _mapper.Map<UserLoggedDto>(validatedUser);
                var tokenInfo = _tokenProvider.BuildToken(validatedUser, systemId);
                userLogged.Role = tokenInfo.Role;
                userLogged.Token = tokenInfo.Token;
                return userLogged;
            }
        }
    }
}