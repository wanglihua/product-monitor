using Core;
using Core.CodeType;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProductInstallMonitorEntryEntityMap : IEntityTypeConfiguration<ProductInstallMonitorEntryEntity>
{
    public void Configure(EntityTypeBuilder<ProductInstallMonitorEntryEntity> builder)
    {
        builder.ToTable("tbl_product_install_monitor_entry");

        EntityMap.Configure(builder);

        builder.Property(e => e.ProductMonitorId)
            .HasColumnName("product_monitor_id")
            .IsRequired(true);

        builder.Property(e => e.LoginUser)
            .HasColumnName("login_user")
            .HasMaxLength(128)
            .IsRequired(true);

        builder.Property(e => e.AccountType)
            .HasConversion<CodeTypeConversion<AccountType>>()
            .HasColumnName("account_type")
            .IsRequired(true);

        builder.Property(e => e.IpAddress)
            .HasColumnName("ip_address")
            .HasMaxLength(32)
            .IsRequired(true);

        builder.Property(e => e.ProvinceType)
            .HasConversion<CodeTypeConversion<ProvinceType>>()
            .HasColumnName("province_type")
            .IsRequired(true);

        builder.Property(e => e.CpuSerialNo)
            .HasColumnName("cpu_serial_no")
            .HasMaxLength(128)
            .IsRequired(true);

        builder.Property(e => e.NetMacAddress)
            .HasColumnName("net_mac_address")
            .HasMaxLength(128)
            .IsRequired(true);
    }
}