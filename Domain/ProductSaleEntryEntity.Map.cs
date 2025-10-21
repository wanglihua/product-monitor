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

        builder.Property(e => e.Version)
            .HasColumnName("version")
            .HasMaxLength(32)
            .IsRequired(true);

        builder.Property(e => e.SaleDate)
            .HasColumnName("sale_date")
            .IsRequired(true);

        builder.Property(e => e.ServiceExpireDate)
            .HasColumnName("service_expire_date")
            .IsRequired(true);

        builder.Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(512)
            .IsRequired(true);
    }
}