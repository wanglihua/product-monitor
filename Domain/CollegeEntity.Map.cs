using Core;
using Core.CodeType;
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
            .HasConversion<CodeTypeConvertions<ProvinceType>>()
            .IsRequired(true);

        builder.Property(e => e.Level)
            .HasColumnName("level")
            .HasConversion<CodeTypeConvertions<CollegeLevelType>>()
            .IsRequired(true);
    }
}