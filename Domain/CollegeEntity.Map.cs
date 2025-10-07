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
            .IsRequired(true);

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .IsRequired(true);

        builder.Property(e => e.Province)
            .HasColumnName("province")
            .IsRequired(true);

        builder.Property(e => e.Level)
            .HasColumnName("level")
            .IsRequired(true);
    }
}