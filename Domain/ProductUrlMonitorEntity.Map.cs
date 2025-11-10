using Core;
using Core.CodeType;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProductUrlMonitorEntityMap : IEntityTypeConfiguration<ProductUrlMonitorEntity>
{
    public void Configure(EntityTypeBuilder<ProductUrlMonitorEntity> builder)
    {
        builder.ToTable("tbl_product_url_monitor");

        EntityMap.Configure(builder);

        builder.Property(e => e.CollegeId)
            .HasColumnName("college_id")
            .IsRequired(true);

        builder.Property(e => e.ProductId)
            .HasColumnName("product_id")
            .IsRequired(true);

        builder.Property(e => e.ProductUrlType)
            .HasConversion<CodeTypeConversion<ProductUrlType>>()
            .HasColumnName("product_url_type")
            .IsRequired(true);

        builder.Property(e => e.TaskCount)
            .HasColumnName("task_count")
            .IsRequired(true);

        builder.Property(e => e.TeacherCount)
            .HasColumnName("teacher_count")
            .IsRequired(true);
    }
}