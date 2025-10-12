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

        builder.Property(e => e.CollegeId)
            .HasColumnName("college_id")
            .IsRequired(true);

        builder.Property(e => e.ProductId)
            .HasColumnName("product_id")
            .IsRequired(true);

        builder.Property(e => e.RentTimes)
            .HasColumnName("rent_times")
            .IsRequired(true);

        builder.Property(e => e.ServiceTimes)
            .HasColumnName("service_times")
            .IsRequired(true);

        builder.Property(e => e.TotalDays)
            .HasColumnName("total_days")
            .IsRequired(true);

        builder.Property(e => e.RemainDays)
            .HasColumnName("remain_days")
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