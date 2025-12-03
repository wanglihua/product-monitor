using Core;
using Core.CodeType;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProductSpecialServiceEntityMap : IEntityTypeConfiguration<ProductSpecialServiceEntity>
{
    public void Configure(EntityTypeBuilder<ProductSpecialServiceEntity> builder)
    {
        builder.ToTable("tbl_product_special_service");

        EntityMap.Configure(builder);

        builder.Property(e => e.CollegeId)
            .HasColumnName("college_id")
            .IsRequired(true);

        builder.Property(e => e.ProductId)
            .HasColumnName("product_id")
            .IsRequired(true);

        builder.Property(e => e.ProjectType)
            .HasColumnName("project_type_code")
            .HasConversion<CodeTypeConversion<ProjectType>>()
            .IsRequired(true);

        builder.Property(e => e.ServiceDate)
            .HasColumnName("service_date")
            .IsRequired(true);

        builder.Property(e => e.ServiceCount)
            .HasColumnName("service_count")
            .IsRequired(true);

        builder.Property(e => e.ServiceUrl)
            .HasColumnName("service_url")
            .HasMaxLength(256)
            .IsRequired(true);

        builder.Property(e => e.LoginUser)
            .HasColumnName("login_user")
            .HasMaxLength(128)
            .IsRequired(true);

        builder.Property(e => e.LoginPassword)
            .HasColumnName("login_password")
            .HasMaxLength(128)
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