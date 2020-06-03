using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UsersAdmin.Core.Model.User;

namespace UsersAdmin.Data.Configurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(e => e.Id)
                     .HasName("PRIMARY");

            builder.ToTable("fwim_user");

            builder.HasIndex(e => e.Id)
                .HasName("fwim_user_i1")
                .IsUnique();

            builder.Property(e => e.Id)
                .HasColumnName("userid")
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            builder.Property(e => e.Description)
                .HasColumnName("userdescri")
                .HasColumnType("varchar(100)")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            builder.Property(e => e.Email)
                .IsRequired()
                .HasColumnName("useremail")
                .HasColumnType("varchar(100)")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("username")
                .HasColumnType("varchar(100)")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            builder.Property(e => e.Pass)
                .IsRequired()
                .HasColumnName("userpass")
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");
        }
    }
}