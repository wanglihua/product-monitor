using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class GroupEntityMap : IEntityTypeConfiguration<GroupEntity>
{
    public void Configure(EntityTypeBuilder<GroupEntity> builder)
    {
        builder.ToTable("tbl_group");

        EntityMap.Configure(builder);

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(128)
            .IsRequired(true);
    }
}