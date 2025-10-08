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

        builder.Property(e => e.VersionUpgradeBase)
            .HasColumnName("version_upgrade_base")
            .HasMaxLength(32)
            .IsRequired(true);

        builder.Property(e => e.VersionUpgradeDesc)
            .HasColumnName("version_upgrade_desc")
            .HasMaxLength(512)
            .IsRequired(true);

        builder.Property(e => e.VersionUpgradeTime)
            .HasColumnName("version_upgrade_time")
            .IsRequired(true);
    }
}