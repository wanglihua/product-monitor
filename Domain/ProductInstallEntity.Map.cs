using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProductInstallEntityMap : IEntityTypeConfiguration<ProductInstallEntity>
{
    public void Configure(EntityTypeBuilder<ProductInstallEntity> builder)
    {
        builder.ToTable("tbl_product_install");

        EntityMap.Configure(builder);

        builder.Property(e => e.CollegeId)
            .HasColumnName("college_id")
            .IsRequired(true);

        builder.Property(e => e.ProductId)
            .HasColumnName("product_id")
            .IsRequired(true);

        builder.Property(e => e.VersionSale)
            .HasColumnName("version_sale")
            .HasMaxLength(32)
            .IsRequired(true);

        builder.Property(e => e.VersionInstall)
            .HasColumnName("version_install")
            .HasMaxLength(32)
            .IsRequired(true);

        builder.Property(e => e.InstallDate)
            .HasColumnName("install_date")
            .IsRequired(true);

        builder.Property(e => e.UpdateUrl)
            .HasColumnName("update_url")
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
    }
}