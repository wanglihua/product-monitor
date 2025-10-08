using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProductUpdateEntityMap : IEntityTypeConfiguration<ProductUpdateEntity>
{
    public void Configure(EntityTypeBuilder<ProductUpdateEntity> builder)
    {
        builder.ToTable("tbl_product_update");

        EntityMap.Configure(builder);

        builder.Property(e => e.ProductId)
            .HasColumnName("product_id")
            .IsRequired(true);
    }
}