using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProductServiceEntityMap : IEntityTypeConfiguration<ProductServiceEntity>
{
    public void Configure(EntityTypeBuilder<ProductServiceEntity> builder)
    {
        builder.ToTable("tbl_product_service");

        EntityMap.Configure(builder);

        builder.Property(e => e.ProductId)
            .HasColumnName("product_id")
            .IsRequired(true);
    }
}