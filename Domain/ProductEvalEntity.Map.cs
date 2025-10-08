using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProductEvalEntityMap : IEntityTypeConfiguration<ProductEvalEntity>
{
    public void Configure(EntityTypeBuilder<ProductEvalEntity> builder)
    {
        builder.ToTable("tbl_product_eval");

        EntityMap.Configure(builder);

        builder.Property(e => e.ProductId)
            .HasColumnName("product_id")
            .IsRequired(true);
    }
}