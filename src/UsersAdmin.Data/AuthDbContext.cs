using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Tatisoft.UsersAdmin.Core.Model.System;
using Tatisoft.UsersAdmin.Core.Model.User;

namespace Tatisoft.UsersAdmin.Data
{
    public partial class AuthDbContext : DbContext
    {
        public virtual DbSet<SystemEntity> Systems { get; set; }
        public virtual DbSet<UserEntity> Users { get; set; }

        public virtual DbSet<UserSystemEntity> UsersSystems { get; set; }

        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
