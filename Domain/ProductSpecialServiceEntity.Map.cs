using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProductSpecialServiceEntityMap : IEntityTypeConfiguration<ProductSpecialServiceEntity>
{
    public void Configure(EntityTypeBuilder<ProductSpecialServiceEntity> builder)
    {
        builder.ToTable("tbl_product_special_service");

        EntityMap.Configure(builder);

        builder.Property(e => e.ProductId)
            .HasColumnName("product_id")
            .IsRequired(true);
    }
}