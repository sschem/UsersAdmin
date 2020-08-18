using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using System.Linq;
using Tatisoft.UsersAdmin.Data;

namespace Tatisoft.UsersAdmin.Test.Integration.Controller.Factory.DataBase
{
    internal class LocalDbTestContext : DefaultDbTestContext
    {
        public LocalDbTestContext(IServiceCollection services, string connectionString) :
            base(services, connectionString)
        { }

        protected override IDbAdapter GetRespawnDbAdapter() => DbAdapter.SqlServer;

        protected override void ConfigureDb()
        {
            this.ConfigureLocalDb();
            base.ConfigureDb();
        }
        
        private void ConfigureLocalDb()
        {
            var dbServiceDescriptor = _services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AuthDbContext>));
            if (dbServiceDescriptor != null)
            {
                _services.Remove(dbServiceDescriptor);
            }

            _services.AddDbContext<AuthDbContext>((options, context) =>
            {
                context.UseSqlServer(_connectionString);
            });
        }
    }
}
