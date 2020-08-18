using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tatisoft.UsersAdmin.Core.Model.User;

namespace Tatisoft.UsersAdmin.Data.Configurations
{
    public class UserSystemEntityConfiguration : IEntityTypeConfiguration<UserSystemEntity>
    {
        public void Configure(EntityTypeBuilder<UserSystemEntity> builder)
        {
            builder.HasKey(e => new { e.UserId, e.SystemId })
                    .HasName("fwim_usrsys_pk");

            builder.ToTable("fwim_usrsys");

            builder.HasIndex(e => e.SystemId)
                     .HasName("fwim_usrsys_i3");

            builder.HasIndex(e => e.UserId)
                .HasName("fwim_usrsys_i2");

            builder.Property(e => e.UserId)
                .HasColumnName("userid")
                .HasColumnType("varchar(50)");

            builder.Property(e => e.SystemId)
                .HasColumnName("systemid")
                .HasColumnType("varchar(50)");

            builder.Property(e => e.Role)
                .IsRequired()
                .HasColumnName("userrole")
                .HasColumnType("varchar(12)")
                .HasConversion(r => r.ToString(), r => (UserRole)Enum.Parse(typeof(UserRole), r));

            builder.HasOne(d => d.System)
                .WithMany(p => p.UserSystemLst)
                .HasForeignKey(d => d.SystemId)
                .HasConstraintName("fk_fwim_usrsys_system");

            builder.HasOne(d => d.User)
                .WithMany(p => p.UserSystemLst)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_fwim_usrsys_user");
        }
    }
}
