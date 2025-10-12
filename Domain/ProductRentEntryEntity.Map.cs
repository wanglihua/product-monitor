using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProductRentEntryEntityMap : IEntityTypeConfiguration<ProductRentEntryEntity>
{
    public void Configure(EntityTypeBuilder<ProductRentEntryEntity> builder)
    {
        builder.ToTable("tbl_product_rent_entry");

        EntityMap.Configure(builder);

        builder.Property(e => e.ProductRentId)
            .HasColumnName("tbl_product_rent_id")
            .IsRequired(true);

        builder.Property(e => e.TotalDays)
            .HasColumnName("total_days")
            .IsRequired(true);

        builder.Property(e => e.BeginDate)
            .HasColumnName("begin_date")
            .IsRequired(true);

        builder.Property(e => e.EndDate)
            .HasColumnName("end_date")
            .IsRequired(true);

        builder.Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(512)
            .IsRequired(true);
    }
}