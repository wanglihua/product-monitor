using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProductRentEntityMap : IEntityTypeConfiguration<ProductRentEntity>
{
    public void Configure(EntityTypeBuilder<ProductRentEntity> builder)
    {
        builder.ToTable("tbl_product_rent");

        EntityMap.Configure(builder);

        builder.Property(e => e.ProductId)
            .HasColumnName("product_id")
            .IsRequired(true);
    }
}