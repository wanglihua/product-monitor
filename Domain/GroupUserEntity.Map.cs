using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class GroupUserEntityMap : IEntityTypeConfiguration<GroupUserEntity>
{
    public void Configure(EntityTypeBuilder<GroupUserEntity> builder)
    {
        builder.ToTable("tbl_group_user");

        EntityNoUpdateMap.Configure(builder);

        builder.Property(e => e.GroupId)
            .HasColumnName("group_id")
            .IsRequired(true);

        builder.Property(e => e.UserId)
            .HasColumnName("user_id")
            .IsRequired(true);
    }
}