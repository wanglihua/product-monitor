using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProductEntityMap : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.ToTable("tbl_product");

        EntityMap.Configure(builder);

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(256)
            .IsRequired(true);

        builder.Property(e => e.EnglishName)
            .HasColumnName("english_name")
            .HasMaxLength(256)
            .IsRequired(true);

        builder.Property(e => e.VersionCount)
            .HasColumnName("version_count")
            .IsRequired(true);

        builder.Property(e => e.LatestVersion)
            .HasColumnName("latest_version")
            .HasMaxLength(32)
            .IsRequired(true);

        builder.Property(e => e.VersionUpdateTime)
            .HasColumnName("version_update_time")
            .IsRequired(true);
    }
}