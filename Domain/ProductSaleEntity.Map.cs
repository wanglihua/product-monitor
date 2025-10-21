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

        builder.Property(e => e.CollegeId)
            .HasColumnName("college_id")
            .IsRequired(true);

        builder.Property(e => e.ProductId)
            .HasColumnName("product_id")
            .IsRequired(true);

        builder.Property(e => e.SaleTimes)
            .HasColumnName("sale_times")
            .IsRequired(true);

        builder.Property(e => e.InstallTimes)
            .HasColumnName("install_times")
            .IsRequired(true);

        builder.Property(e => e.UpdateTimes)
            .HasColumnName("update_times")
            .IsRequired(true);

        builder.Property(e => e.ServiceTimes)
            .HasColumnName("service_times")
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