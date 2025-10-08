using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProductUrlEntityMap : IEntityTypeConfiguration<ProductUrlEntity>
{
    public void Configure(EntityTypeBuilder<ProductUrlEntity> builder)
    {
        builder.ToTable("tbl_product_url");
        
        EntityMap.Configure(builder);

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(128)
            .IsRequired(true);
    }
}