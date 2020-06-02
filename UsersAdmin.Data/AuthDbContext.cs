using System;
using Microsoft.EntityFrameworkCore;
using UsersAdmin.Core.Model.System;
using UsersAdmin.Data.Configurations;

namespace UsersAdmin.Data
{
    public partial class AuthDbContext : DbContext
    {
        public virtual DbSet<SystemEntity> Systems { get; set; }

        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SystemEntityConfiguration());
        }
    }
}
