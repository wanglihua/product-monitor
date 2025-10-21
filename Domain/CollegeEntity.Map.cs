
using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class CollegeEntityMap : IEntityTypeConfiguration<CollegeEntity>
{
    public void Configure(EntityTypeBuilder<CollegeEntity> builder)
    {
        builder.ToTable("tbl_college");

        EntityMap.Configure(builder);

        builder.Property(e => e.Code)
            .HasColumnName("code")
            .HasMaxLength(64)
            .IsRequired(true);

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(128)
            .IsRequired(true);

        builder.Property(e => e.Province)
            .HasColumnName("province")
            .HasMaxLength(32)
            .IsRequired(true);

        builder.Property(e => e.Level)
            .HasColumnName("level")
            .HasMaxLength(32)
            .IsRequired(true);
    }
}