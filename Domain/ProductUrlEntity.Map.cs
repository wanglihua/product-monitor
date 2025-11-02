using Core;
using Core.CodeType;
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

        builder.Property(e => e.Url)
            .HasColumnName("url")
            .HasMaxLength(256)
            .IsRequired(true);

        builder.Property(e => e.Type)
            .HasColumnName("type_code")
            .HasConversion<CodeTypeValueConverter<ProductUrlType>>()
            .IsRequired(true);

        builder.Property(e => e.LoginUser)
            .HasColumnName("login_user")
            .HasMaxLength(128)
            .IsRequired(true);

        builder.Property(e => e.LoginPassword)
            .HasColumnName("login_password")
            .HasMaxLength(128)
            .IsRequired(true);

        builder.Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(512)
            .IsRequired(true);
    }
}