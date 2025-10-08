using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProductSaleEntryEntityMap : IEntityTypeConfiguration<ProductSaleEntryEntity>
{
    public void Configure(EntityTypeBuilder<ProductSaleEntryEntity> builder)
    {
        builder.ToTable("tbl_product_sale_entry");

        EntityMap.Configure(builder);

        builder.Property(e => e.ProductSaleId)
            .HasColumnName("product_sale_id")
            .IsRequired(true);
    }
}