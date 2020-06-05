using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UsersAdmin.Core.Model.System;
using UsersAdmin.Core.Model.User;
using UsersAdmin.Data;

namespace UsersAdmin.Test.Unit.Repository
{
    public class Testing
    {
        public static DbContextOptions<AuthDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<AuthDbContext>();
            builder.UseInMemoryDatabase("AutDB")
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        public static SystemEntity GetValidSystemEntity() => new SystemEntity()
        {
            Id = "SystemValidId",
            Name = "System Valid Name",
            Description = "System Valid Description"
        };

        public static UserEntity GetValidUserEntity() => new UserEntity()
        {
            Id = "UserValidId",
            Name = "User Valid Name",
            Description = "User Valid Description",
            Email = "validuser@mail.com",
            Pass = "validclearpass"            
        };
    }
}
