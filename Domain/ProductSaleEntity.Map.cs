using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProductSaleEntityMap : IEntityTypeConfiguration<ProductSaleEntity>
{
    public void Configure(EntityTypeBuilder<ProductSaleEntity> builder)
    {
        builder.ToTable("tbl_product_sale");

        EntityMap.Configure(builder);

        builder.Property(e => e.ProductId)
            .HasColumnName("product_id")
            .IsRequired(true);
    }
}