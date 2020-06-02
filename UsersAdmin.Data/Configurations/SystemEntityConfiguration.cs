using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UsersAdmin.Core.Model.System;

namespace UsersAdmin.Data.Configurations
{
    public class SystemEntityConfiguration : IEntityTypeConfiguration<SystemEntity>
    {
        public void Configure(EntityTypeBuilder<SystemEntity> builder)
        {
            builder.HasKey(e => e.Id)
                 .HasName("PRIMARY");

            builder.ToTable("fwim_system");

            builder.HasIndex(e => e.Id)
                .HasName("fwim_system_i1")
                .IsUnique();

            builder.Property(e => e.Id)
                .HasColumnName("systemid")
                .HasColumnType("varchar(50)");

            builder.Property(e => e.Description)
                .HasColumnName("systemdescri")
                .HasColumnType("varchar(100)");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("systemname")
                .HasColumnType("varchar(100)");
        }
    }
}