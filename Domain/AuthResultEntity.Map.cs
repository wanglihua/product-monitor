using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class AuthResultEntityMap : IEntityTypeConfiguration<AuthResultEntity>
{
    public void Configure(EntityTypeBuilder<AuthResultEntity> builder)
    {
        // builder.ToTable("view_auth_result");
        builder.ToView("view_auth_result");

        EntityNoUpdateMap.Configure(builder);

        builder.Property(e => e.PassResCateId)
            .HasColumnName("pass_res_cate_id")
            .IsRequired(true);

        builder.Property(e => e.PassResCateName)
            .HasColumnName("pass_res_cate_name")
            .HasMaxLength(128)
            .IsRequired(true);

        builder.Property(e => e.PassResId)
            .HasColumnName("pass_res_id")
            .IsRequired(true);

        builder.Property(e => e.PassResName)
            .HasColumnName("pass_res_name")
            .HasMaxLength(128)
            .IsRequired(true);

        builder.Property(e => e.UserId)
            .HasColumnName("user_id")
            .IsRequired(true);

        builder.Property(e => e.LoginName)
            .HasColumnName("login_name")
            .HasMaxLength(128)
            .IsRequired(true);

        builder.Property(e => e.UserName)
            .HasColumnName("user_name")
            .HasMaxLength(128)
            .IsRequired(true);

        builder.Property(e => e.GroupId)
            .HasColumnName("group_id")
            .IsRequired(false);

        builder.Property(e => e.GroupName)
            .HasColumnName("group_name")
            .HasMaxLength(128)
            .IsRequired(true);
    }
}