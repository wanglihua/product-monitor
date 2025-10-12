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

        builder.Property(e => e.EvalTimes)
            .HasColumnName("eval_times")
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