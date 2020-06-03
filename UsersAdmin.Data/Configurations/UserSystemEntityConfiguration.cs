using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UsersAdmin.Core.Model.User;

namespace UsersAdmin.Data.Configurations
{
    public class UserSystemEntityConfiguration : IEntityTypeConfiguration<UserSystemEntity>
    {
        public void Configure(EntityTypeBuilder<UserSystemEntity> builder)
        {
            builder.HasKey(e => new { e.UserId, e.SystemId })
                    .HasName("PRIMARY");

            builder.ToTable("fwim_usrsys");

            builder.HasIndex(e => e.SystemId)
                     .HasName("fwim_usrsys_i3");

            builder.HasIndex(e => e.UserId)
                .HasName("fwim_usrsys_i2");

            builder.Property(e => e.UserId)
                .HasColumnName("userid")
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            builder.Property(e => e.SystemId)
                .HasColumnName("systemid")
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

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
