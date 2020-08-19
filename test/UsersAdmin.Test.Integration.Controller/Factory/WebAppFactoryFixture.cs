using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Tatisoft.UsersAdmin.Core.Model.System;
using Tatisoft.UsersAdmin.Core.Model.User;
using Tatisoft.UsersAdmin.Core.Repositories;
using Tatisoft.UsersAdmin.Core.Security;
using Tatisoft.UsersAdmin.Test.Integration.Controller.Factory.DataBase;

namespace Tatisoft.UsersAdmin.Test.Integration.Controller.Factory
{
    public class WebAppFactoryFixture : WebApplicationFactory<Tatisoft.UsersAdmin.Api.Startup>
    {
        protected readonly string MEDIA_TYPE = "application/json";
        protected readonly Encoding ENCODING = Encoding.UTF8;
        private readonly string _localConnectionStringName = "AuthDbLocalSql";
        public readonly string CONTENT_TYPE = "application/json; charset=utf-8";

        public IServiceScopeFactory ScopeFactory { get; private set; }
        public UserLoggedDto UserAdmin { get; private set; }
        public UserLoggedDto UserSystemAdmin { get; private set; }
        public UserLoggedDto UserUser { get; private set; }

        public IMapper MapperInstance => _repoHelper.MapperInstance;

        private ITokenProvider _tokenProvider;
        private IConfiguration _configuration;
        private IDbTestContext _dbContext;
        private RepositoryTestHelper _repoHelper;

        public WebAppFactoryFixture() : base()
        {
            //To execute ConfigureWebHost method at the very beginning
            this.CreateClient();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            LoadLocalConfigurationFile(builder);
            builder.ConfigureServices(services =>
            {
                this.SetUpDbContext(services);
                this.ScopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
                _repoHelper = new RepositoryTestHelper(this.ScopeFactory);
                SetupSecurity(services);
            });
        }

        private void LoadLocalConfigurationFile(IWebHostBuilder builder)
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.json");
            builder.ConfigureAppConfiguration((context, conf) =>
            {
                conf.AddJsonFile(configPath);
                _configuration = conf.Build();
            });
        }

        private void SetUpDbContext(IServiceCollection services)
        {
            var useLocalSqlDb = _configuration.GetValue<bool>("UseSqlLocalDb");
            if (useLocalSqlDb)
            {
                var connString = _configuration.GetConnectionString(_localConnectionStringName);
                _dbContext = new LocalDbTestContext(services, connString);
            }
            else
            {
                _dbContext = new DefaultDbTestContext(services);
            }
        }

        private void SetupSecurity(IServiceCollection services)
        {
            //Used to give the config value.
            //var jwtConfig = _configuration.GetSection("JwtConfig").Get<JwtConfig>();
            _tokenProvider = services.BuildServiceProvider().GetService<ITokenProvider>();

            UserEntity userEntity = new UserEntity() { Id = "ADMIN", Name = "Administrator", IsAdmin = true, Email = "admin@test.com", Pass = "x" };
            Task.Run(() => _repoHelper.InsertEntity<UserEntity>(userEntity)).Wait();
            this.UserAdmin = _repoHelper.MapperInstance.Map<UserLoggedDto>(userEntity);
            var tokenInfo = _tokenProvider.BuildToken(userEntity, null);
            this.UserAdmin.Token = tokenInfo.Token;
            this.UserAdmin.Role = tokenInfo.Role;

            userEntity = new UserEntity() { Id = "SYS_ADM", Name = "System Admin", IsAdmin = false, Email = "admin@test.com", Pass = "x" };
            var systemEntity = new SystemEntity() { Id = "TEST_SYSTEM_ADM", Name = "System for SystemAdmin" };
            var userSystemEntity = new UserSystemEntity
            {
                SystemId = systemEntity.Id,
                System = systemEntity,
                UserId = userEntity.Id,
                User = userEntity,
                Role = UserRole.SystemAdmin
            };
            this.UserSystemAdmin = _repoHelper.MapperInstance.Map<UserLoggedDto>(userEntity);
            Task.Run(() => _repoHelper.InsertEntity<UserSystemEntity>(userSystemEntity)).Wait();
            tokenInfo = _tokenProvider.BuildToken(userEntity, systemEntity.Id);
            this.UserSystemAdmin.Token = tokenInfo.Token;
            this.UserSystemAdmin.Role = tokenInfo.Role;

            userEntity = new UserEntity() { Id = "USER", Name = "System User", IsAdmin = false, Email = "user@test.com", Pass = "x" };
            systemEntity = new SystemEntity() { Id = "TEST_SYSTEM_USER", Name = "System for User" };
            userSystemEntity = new UserSystemEntity
            {
                SystemId = systemEntity.Id,
                System = systemEntity,
                UserId = userEntity.Id,
                User = userEntity,
                Role = UserRole.User
            };
            this.UserUser = _repoHelper.MapperInstance.Map<UserLoggedDto>(userEntity);
            Task.Run(() => _repoHelper.InsertEntity<UserSystemEntity>(userSystemEntity)).Wait();
            tokenInfo = _tokenProvider.BuildToken(userEntity, systemEntity.Id);
            this.UserUser.Token = tokenInfo.Token;
            this.UserUser.Role = tokenInfo.Role;
        }

        public StringContent CreateMessageContent(object dto)
        {
            var jsonDto = JsonConvert.SerializeObject(dto);
            var msgContent = new StringContent(jsonDto, this.ENCODING, this.MEDIA_TYPE);
            return msgContent;
        }

        public Task ClearCache(string cacheKey)
        {
            using var scope = ScopeFactory.CreateScope();
            var cache = scope.ServiceProvider.GetService<IDistributedCache>();
            return cache.RemoveAsync(cacheKey);
        }

        public HttpClient CreateAuthenticatedAsAdminClient()
        {
            var client = this.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.UserAdmin.Token);
            return client;
        }

        public HttpClient CreateAuthenticatedAsSystemAdminClient()
        {
            var client = this.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.UserSystemAdmin.Token);
            return client;
        }

        public HttpClient CreateAuthenticatedAsUserClient()
        {
            var client = this.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.UserUser.Token);
            return client;
        }

        public async Task AddDto<TEntity, TDto>(TDto dto)
           where TEntity : class => await _repoHelper.InsertDto<TEntity, TDto>(dto);

        public async ValueTask<TEntity> FindAsync<TEntity, TDto>(TDto dto)
            where TEntity : class, IIds => await _repoHelper.SelectAsync<TEntity, TDto>(dto);

        public async Task AddEntity<TEntity>(TEntity entity)
            where TEntity : class => await _repoHelper.InsertEntity(entity);

        protected override void Dispose(bool disposing)
        {
            _dbContext.Reset(this.ScopeFactory);
            base.Dispose(disposing);
        }
    }
}
