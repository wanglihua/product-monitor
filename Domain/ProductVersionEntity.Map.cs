using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProductVersionEntityMap : IEntityTypeConfiguration<ProductVersionEntity>
{
    public void Configure(EntityTypeBuilder<ProductVersionEntity> builder)
    {
        builder.ToTable("tbl_product_version");

        EntityMap.Configure(builder);

        builder.Property(e => e.ProductId)
            .HasColumnName("product_id")
            .IsRequired(true);

        builder.Property(e => e.Version)
            .HasColumnName("version")
            .HasMaxLength(32)
            .IsRequired(true);

        builder.Property(e => e.VersionUpdateBase)
            .HasColumnName("version_update_base")
            .HasMaxLength(32)
            .IsRequired(true);

        builder.Property(e => e.VersionUpdateDesc)
            .HasColumnName("version_update_desc")
            .HasMaxLength(512)
            .IsRequired(true);

        builder.Property(e => e.VersionUpdateTime)
            .HasColumnName("version_update_time")
            .IsRequired(true);
    }
}