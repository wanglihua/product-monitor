using Core;
using Core.CodeType;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProductUrlMonitorEntryEntityMap : IEntityTypeConfiguration<ProductUrlMonitorEntryEntity>
{
    public void Configure(EntityTypeBuilder<ProductUrlMonitorEntryEntity> builder)
    {
        builder.ToTable("tbl_product_url_monitor_entry");

        EntityMap.Configure(builder);

        builder.Property(e => e.ProductUrlMonitorId)
            .HasColumnName("product_url_monitor_id")
            .IsRequired(true);

        builder.Property(e => e.LoginUser)
            .HasColumnName("login_user")
            .HasMaxLength(128)
            .IsRequired(true);

        builder.Property(e => e.LoginPassword)
            .HasColumnName("login_password")
            .HasMaxLength(128)
            .IsRequired(true);

        builder.Property(e => e.IpAddress)
            .HasColumnName("ip_address")
            .HasMaxLength(32)
            .IsRequired(true);

        builder.Property(e => e.ProvinceType)
            .HasConversion<CodeTypeConvertions<ProvinceType>>()
            .HasColumnName("province_type")
            .IsRequired(true);
    }
}