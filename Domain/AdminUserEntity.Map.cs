using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class AdminUserEntityMap : IEntityTypeConfiguration<AdminUserEntity>
{
    public void Configure(EntityTypeBuilder<AdminUserEntity> builder)
    {
        builder.ToTable("tbl_admin_user");

        EntityNoUpdateMap.Configure(builder);

        builder.Property(e => e.UserId)
            .HasColumnName("user_id")
            .IsRequired(true);
    }
}