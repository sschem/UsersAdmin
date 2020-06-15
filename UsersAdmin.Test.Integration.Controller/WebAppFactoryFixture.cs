using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Respawn;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UsersAdmin.Core.Model.Mapping;
using UsersAdmin.Core.Repositories;
using UsersAdmin.Data;

namespace UsersAdmin.Test.Integration.Controller
{
    public class WebAppFactoryFixture : WebApplicationFactory<UsersAdmin.Api.Startup>
    {
        protected readonly string MEDIA_TYPE = "application/json";
        protected readonly Encoding ENCODING = Encoding.UTF8;
        public readonly IMapper MapperInstance;

        private Checkpoint _checkpoint;
        private IConfiguration _configuration;

        private bool _useLocalSqlDb;
        
        private readonly string _localConnectionStringName = "AuthDbLocalSql";
        public readonly string CONTENT_TYPE = "application/json; charset=utf-8";

        private IDbAdapter RespawnDbAdapter => _useLocalSqlDb ? DbAdapter.SqlServer : DbAdapter.MySql;

        public IServiceScopeFactory ScopeFactory { get; private set; }

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
                _useLocalSqlDb = _configuration.GetValue<bool>("UseSqlLocalDb");

                if (_useLocalSqlDb)
                    UseTestDb(services);

                ConfigureDb(services);
                this.ScopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
            });
        }

        protected override void Dispose(bool disposing)
        {
            this.Reset();
            base.Dispose(disposing);
        }

        public void Reset()
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<AuthDbContext>();
                var dbConnection = dbContext.Database.GetDbConnection();
                dbConnection.Open();
                _checkpoint.Reset(dbConnection).Wait();
            }
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

        private void ConfigureDb(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var dbContext = scopedServices.GetRequiredService<AuthDbContext>();
                dbContext.Database.EnsureCreated();
            }

            _checkpoint = new Checkpoint() { DbAdapter = RespawnDbAdapter };
        }

        private void UseTestDb(IServiceCollection services)
        {
            var dbServiceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AuthDbContext>));
            if (dbServiceDescriptor != null)
            {
                services.Remove(dbServiceDescriptor);
            }

            services.AddDbContext<AuthDbContext>((options, context) =>
            {
                context.UseSqlServer(_configuration.GetConnectionString(_localConnectionStringName));
            });
        }

        public void AddDto<TEntity, TDto>(TDto dto)
            where TEntity : class
        {
            this.AddEntity(MapperInstance.Map<TEntity>(dto));
        }

        public void AddEntity<TEntity>(TEntity entity)
            where TEntity : class
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<AuthDbContext>();
                dbContext.Add(entity);
                dbContext.SaveChanges();
            }
        }

        public async Task<TEntity> FindAsync<TEntity, TDto>(TDto dto)
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
    }
}
