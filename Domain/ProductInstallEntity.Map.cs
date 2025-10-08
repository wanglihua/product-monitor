using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProductInstallEntityMap : IEntityTypeConfiguration<ProductInstallEntity>
{
    public void Configure(EntityTypeBuilder<ProductInstallEntity> builder)
    {
        builder.ToTable("tbl_product_install");

        EntityMap.Configure(builder);

        builder.Property(e => e.ProductId)
            .HasColumnName("product_id")
            .IsRequired(true);
    }
}