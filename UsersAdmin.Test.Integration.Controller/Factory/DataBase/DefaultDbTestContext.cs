using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using UsersAdmin.Data;

namespace UsersAdmin.Test.Integration.Controller.Factory.DataBase
{
    internal class DefaultDbTestContext : IDbTestContext
    {
        protected readonly IServiceCollection _services;
        protected readonly string _connectionString;

        private Checkpoint _checkpoint;

        internal DefaultDbTestContext(IServiceCollection services, string connectionString = null)
        {
            _services = services;
            _connectionString = connectionString;
            this.ConfigureDb();
        }

        protected virtual IDbAdapter GetRespawnDbAdapter() => DbAdapter.MySql;

        protected virtual void ConfigureDb()
        {
            var serviceProvider = _services.BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var dbContext = scopedServices.GetRequiredService<AuthDbContext>();
                dbContext.Database.EnsureCreated();
            }

            _checkpoint = new Checkpoint() { DbAdapter = this.GetRespawnDbAdapter() };
        }

        public void Reset(IServiceScopeFactory scopeFactory)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<AuthDbContext>();
                var dbConnection = dbContext.Database.GetDbConnection();
                dbConnection.Open();
                _checkpoint.Reset(dbConnection).Wait();
            }
        }
    }
}
