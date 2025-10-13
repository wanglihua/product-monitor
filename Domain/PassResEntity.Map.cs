using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class PassResEntityMap : IEntityTypeConfiguration<PassResEntity>
{
    public void Configure(EntityTypeBuilder<PassResEntity> builder)
    {
        builder.ToTable("tbl_pass_res");

        EntityAssignedIdMap.Configure(builder);

        builder.Property(e => e.CateId)
            .HasColumnName("cate_id")
            .IsRequired(true);

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(128)
            .IsRequired(true);
    }
}