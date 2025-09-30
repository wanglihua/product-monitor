using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class GroupPassResEntityMap : IEntityTypeConfiguration<GroupPassResEntity>
{
    public void Configure(EntityTypeBuilder<GroupPassResEntity> builder)
    {
        builder.ToTable("tbl_group_pass_res");

        EntityNoUpdateMap.Configure(builder);

        builder.Property(e => e.GroupId)
            .HasColumnName("group_id")
            .IsRequired(true);

        builder.Property(e => e.PassResId)
            .HasColumnName("pass_res_id")
            .IsRequired(true);
    }
}