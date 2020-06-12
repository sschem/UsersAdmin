using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UsersAdmin.Data;

namespace UsersAdmin.Test.Integration.Controller
{
    public class ControllerAppFactory : WebApplicationFactory<UsersAdmin.Api.Startup>
    {
        private Checkpoint _checkpoint;
        private IConfiguration _configuration;
        
        private readonly bool _useLocalSqlDb;
        private readonly string _localConnectionStringName = "AuthDbLocalSql";

        private IDbAdapter RespawnDbAdapter => _useLocalSqlDb ? DbAdapter.SqlServer : DbAdapter.MySql;

        public IServiceScopeFactory ScopeFactory { get; private set; }

        public ControllerAppFactory(bool useLocalSqlDb)
        {
            _useLocalSqlDb = useLocalSqlDb;
        }

        public async Task Reset()
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<AuthDbContext>();
                var dbConnection = dbContext.Database.GetDbConnection();
                dbConnection.Open();
                await _checkpoint.Reset(dbConnection);
            }
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            LoadLocalConfigurationFile(builder);
            builder.ConfigureServices(services =>
            {
                if (_useLocalSqlDb) 
                    UseTestDb(services);
                ConfigureDb(services);
                this.ScopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
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
    }
}
