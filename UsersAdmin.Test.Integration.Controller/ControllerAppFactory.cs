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
        private readonly string _connectionStringName = "AuthDbTest";

        public IServiceScopeFactory ScopeFactory { get; private set; }

        public async Task Reset()
        {
            await _checkpoint.Reset(_configuration.GetConnectionString(_connectionStringName));
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            LoadLocalConfigurationFile(builder);
            builder.ConfigureServices(services =>
            {
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

            _checkpoint = new Checkpoint();
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
                context.UseSqlServer(_configuration.GetConnectionString(_connectionStringName));
            });
        }        
    }
}
