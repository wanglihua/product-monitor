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