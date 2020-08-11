using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Respawn;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UsersAdmin.Api.Config;
using UsersAdmin.Core.Model.Mapping;
using UsersAdmin.Core.Model.User;
using UsersAdmin.Core.Repositories;
using UsersAdmin.Core.Security;
using UsersAdmin.Data;
using UsersAdmin.Test.Integration.Controller.Factory.DataBase;

namespace UsersAdmin.Test.Integration.Controller.Factory
{
    public class WebAppFactoryFixture : WebApplicationFactory<UsersAdmin.Api.Startup>
    {
        protected readonly string MEDIA_TYPE = "application/json";
        protected readonly Encoding ENCODING = Encoding.UTF8;                
        private readonly string _localConnectionStringName = "AuthDbLocalSql";
        public readonly string CONTENT_TYPE = "application/json; charset=utf-8";
        
        public IServiceScopeFactory ScopeFactory { get; private set; }
        public UserLoggedDto UserAdmin { get; private set; }
        public readonly IMapper MapperInstance;

        private ITokenProvider _tokenProvider;        
        private IConfiguration _configuration;
        private IDbTestContext _dbContext;
        
        public WebAppFactoryFixture() : base()
        {
            //To execute ConfigureWebHost method at the very beginning
            this.CreateClient();

            MapperInstance = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            })
            .CreateMapper();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            LoadLocalConfigurationFile(builder);
            builder.ConfigureServices(services =>
            {
                this.SetUpDbContext(services);
                this.ScopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
                ConfigureAuth(services);
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

        private void ConfigureAuth(IServiceCollection services)
        {
            //Used to give the config value.
            //var jwtConfig = _configuration.GetSection("JwtConfig").Get<JwtConfig>();
            _tokenProvider = services.BuildServiceProvider().GetService<ITokenProvider>();
            this.UserAdmin = new UserLoggedDto() { Id = "ADMIN", Name = "Administrator", Role = "Admin" };
            this.UserAdmin.Token = _tokenProvider.BuildToken(this.UserAdmin);
        }

        public async Task AddDto<TEntity, TDto>(TDto dto)
            where TEntity : class
        {
            var entity = MapperInstance.Map<TEntity>(dto);
            await this.AddEntity(entity);
        }

        public async Task AddEntity<TEntity>(TEntity entity)
            where TEntity : class
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<AuthDbContext>();
                await dbContext.AddAsync(entity);
                await dbContext.SaveChangesAsync();
            }
        }

        public async ValueTask<TEntity> FindAsync<TEntity, TDto>(TDto dto)
            where TEntity : class, IIds
        {
            TEntity entity = MapperInstance.Map<TEntity>(dto);
            using (var scope = ScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<AuthDbContext>();
                return await dbContext.FindAsync<TEntity>(entity.GetIds);
            }
        }

        public StringContent CreateMessageContent(object dto)
        {
            var jsonDto = JsonConvert.SerializeObject(dto);
            var msgContent = new StringContent(jsonDto, this.ENCODING, this.MEDIA_TYPE);
            return msgContent;
        }

        public Task ClearCache(string cacheKey)
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var cache = scope.ServiceProvider.GetService<IDistributedCache>();
                return cache.RemoveAsync(cacheKey);
            }
        }
        
        new public HttpClient CreateClient()
        {
            var client = base.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.UserAdmin.Token);
            return client;
        }

        protected override void Dispose(bool disposing)
        {
            _dbContext.Reset(this.ScopeFactory);
            base.Dispose(disposing);
        }
    }
}
