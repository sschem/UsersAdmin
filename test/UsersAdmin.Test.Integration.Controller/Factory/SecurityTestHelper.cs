using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Tatisoft.UsersAdmin.Core.Model.System;
using Tatisoft.UsersAdmin.Core.Model.User;
using Tatisoft.UsersAdmin.Core.Security;


namespace Tatisoft.UsersAdmin.Test.Integration.Controller.Factory
{
    public class SecurityTestHelper
    {
        public UserLoggedDto UserAdmin { get; private set; }
        public UserLoggedDto UserSystemAdmin { get; private set; }
        public UserLoggedDto UserUser { get; private set; }

        private readonly ITokenProvider _tokenProvider;

        private readonly RepositoryTestHelper _repoHelper;

        public SecurityTestHelper(IServiceCollection services, RepositoryTestHelper repoHelper)
        {
            //var jwtConfig = _configuration.GetSection("JwtConfig").Get<JwtConfig>();
            _tokenProvider = services.BuildServiceProvider().GetService<ITokenProvider>();
            _repoHelper = repoHelper;
            this.ConfigureAdminUser();
            this.ConfigureSystemAdminUser();
            this.ConfigureUserUser();
        }

        private void ConfigureAdminUser()
        {
            UserEntity userEntity = new UserEntity() { Id = "ADMIN", Name = "Administrator", IsAdmin = true, Email = "admin@test.com", Pass = "x" };
            
            Task.Run(() => _repoHelper.InsertEntity<UserEntity>(userEntity)).Wait();
            this.UserAdmin = this.BuildUserLogged(userEntity, null);
        }

        private void ConfigureSystemAdminUser()
        {
            var userEntity = new UserEntity() { Id = "SYS_ADM", Name = "System Admin", IsAdmin = false, Email = "admin@test.com", Pass = "x" };
            var systemEntity = new SystemEntity() { Id = "TEST_SYSTEM_ADM", Name = "System for SystemAdmin" };
            var userSystemEntity = new UserSystemEntity
            {
                SystemId = systemEntity.Id,
                System = systemEntity,
                UserId = userEntity.Id,
                User = userEntity,
                Role = UserRole.SystemAdmin
            };
            
            Task.Run(() => _repoHelper.InsertEntity<UserSystemEntity>(userSystemEntity)).Wait();
            this.UserSystemAdmin = this.BuildUserLogged(userEntity, systemEntity.Id);
        }

        private void ConfigureUserUser()
        {
            var userEntity = new UserEntity() { Id = "USER", Name = "System User", IsAdmin = false, Email = "user@test.com", Pass = "x" };
            var systemEntity = new SystemEntity() { Id = "TEST_SYSTEM_USER", Name = "System for User" };
            var userSystemEntity = new UserSystemEntity
            {
                SystemId = systemEntity.Id,
                System = systemEntity,
                UserId = userEntity.Id,
                User = userEntity,
                Role = UserRole.User
            };

            Task.Run(() => _repoHelper.InsertEntity<UserSystemEntity>(userSystemEntity)).Wait();
            this.UserUser = this.BuildUserLogged(userEntity, systemEntity.Id);
        }

        private UserLoggedDto BuildUserLogged(UserEntity userEntity, string systemId)
        {
            var user = _repoHelper.MapperInstance.Map<UserLoggedDto>(userEntity);
            var tokenInfo = _tokenProvider.BuildToken(userEntity, systemId);
            user.Token = tokenInfo.Token;
            user.Role = tokenInfo.Role;
            return user;
        }
    }
}
