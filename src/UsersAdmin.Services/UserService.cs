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
        private readonly ISystemRepository _systemRepository;

        public virtual string UserIncorrect { get { return "Datos de Usuario incorrectos!"; } }

        public virtual string UserSystemIncorrect { get { return "Datos de Usuario/Sistema incorrectos!"; } }

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IAppCache cache, ITokenProvider tokenProvider, ISystemRepository systemRepository)
            : base(unitOfWork, mapper, cache)
        {
            _tokenProvider = tokenProvider;
            _systemRepository = systemRepository;
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

        public async Task<UserDto> GetBySystemAsync(string userId, string systemId)
        {
            var userEntity = await this.GetUserWithSystem(userId, systemId);
            return _mapper.Map<UserDto>(userEntity);
        }

        public async Task AssociateUserSystemAsync(string userId, string systemId)
        {
            var userEntity = this.Repository.SelectIncludingSystems(userId);
            var systemEntity = await _systemRepository.SelectByIdAsync(systemId);
            if (userEntity != null && systemEntity != null)
            {
                bool systemAlreadyAssociated = userEntity.UserSystemLst.Where(us => us.SystemId == systemId).Count() > 0;
                if (!systemAlreadyAssociated)
                {
                    UserSystemEntity userSystemEntity = new UserSystemEntity()
                    {
                        User = userEntity,
                        UserId = userId,
                        System = systemEntity,
                        SystemId = systemId
                    };
                    userEntity.UserSystemLst.Add(userSystemEntity);
                    this.Repository.Update(userEntity);
                    await _unitOfWork.CommitAsync();
                }
            }
            else
            {
                throw new WarningException(this.UserSystemIncorrect);
            }
        }

        public async Task UnassociateUserSystemAsync(string userId, string systemId)
        {
            var userEntity = this.Repository.SelectIncludingSystems(userId);
            UserSystemEntity systemToRemove = null;
            
            if (userEntity != null)
            {
                systemToRemove = userEntity.UserSystemLst
                    .Where(s => s.SystemId == systemId)
                    .FirstOrDefault();
                
                if (systemToRemove != null)
                {
                    userEntity.UserSystemLst.Remove(systemToRemove);
                    this.Repository.Update(userEntity);
                    await _unitOfWork.CommitAsync();
                }
            }

            if (userEntity == null || systemToRemove == null)
            {
                throw new WarningException(this.UserSystemIncorrect);
            }
        }

        private async Task<UserEntity> GetUserWithSystem(string userId, string systemId)
        {
            var entities = await this.GetAllEntitiesAsync();
            var userEntity = entities
                .Where(u => u.Id == userId && !string.IsNullOrWhiteSpace(systemId))
                .FirstOrDefault();

            if (userEntity != null)
            {
                userEntity = this.Repository.SelectIncludingSystems(userEntity.Id);
                userEntity.UserSystemLst = userEntity.UserSystemLst
                    .Where(s => s.SystemId == systemId)
                    .ToList();
            }

            if (userEntity == null || userEntity.UserSystemLst.Count == 0)
            {
                throw new WarningException(this.UserSystemIncorrect);
            }

            return userEntity;
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