using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProductEvalEntryEntityMap : IEntityTypeConfiguration<ProductEvalEntryEntity>
{
    public void Configure(EntityTypeBuilder<ProductEvalEntryEntity> builder)
    {
        builder.ToTable("tbl_product_eval_entry");

        EntityMap.Configure(builder);

        builder.Property(e => e.ProductEvalId)
            .HasColumnName("product_eval_id")
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

        builder.Property(e => e.SalesManager)
            .HasColumnName("sales_manager")
            .HasMaxLength(32)
            .IsRequired(true);

        builder.Property(e => e.ContactTeacher)
            .HasColumnName("contact_teacher")
            .HasMaxLength(32)
            .IsRequired(true);

        builder.Property(e => e.ContactMobile)
            .HasColumnName("contact_mobile")
            .HasMaxLength(32)
            .IsRequired(true);

        builder.Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(512)
            .IsRequired(true);

        builder.Property(e => e.UserId)
            .HasColumnName("user_id")
            .IsRequired(true);
    }
}