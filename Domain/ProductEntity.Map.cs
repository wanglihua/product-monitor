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
            .IsRequired(true);

        builder.Property(e => e.Version)
            .HasColumnName("version")
            .IsRequired(true);
        
        builder.Property(e => e.VersionUpgradeBase)
            .HasColumnName("version_upgrade_base")
            .IsRequired(true);
        
        builder.Property(e => e.VersionUpgradeDesc)
            .HasColumnName("version_upgrade_desc")
            .IsRequired(true);
        
        builder.Property(e => e.VersionUpgradeTime)
            .HasColumnName("version_upgrade_time")
            .IsRequired(true);
    }
}