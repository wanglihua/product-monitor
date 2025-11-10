using Core;
using Core.CodeType;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class UserEntityMap : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("tbl_user");

        EntityMap.Configure(builder);

        builder.Property(e => e.LoginName)
            .HasColumnName("login_name")
            .HasMaxLength(128)
            .IsRequired(true);

        builder.Property(e => e.UserName)
            .HasColumnName("user_name")
            .HasMaxLength(128)
            .IsRequired(true);

        builder.Property(e => e.Password)
            .HasColumnName("password")
            .HasMaxLength(128)
            .IsRequired(true);

        builder.Property(e => e.Salt)
            .HasColumnName("salt")
            .HasMaxLength(128)
            .IsRequired(true);

        builder.Property(e => e.LastLoginTime)
            .HasColumnName("last_login_time")
            .IsRequired(false);

        builder.Property(e => e.Status)
            .HasConversion<CodeTypeConversion<UserStatus>>()
            .HasColumnName("status_id")
            .IsRequired(false);
    }
}