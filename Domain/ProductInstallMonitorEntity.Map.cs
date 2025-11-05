using Core;
using Core.CodeType;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProductInstallMonitorEntityMap : IEntityTypeConfiguration<ProductInstallMonitorEntity>
{
    public void Configure(EntityTypeBuilder<ProductInstallMonitorEntity> builder)
    {
        builder.ToTable("tbl_product_install_monitor");

        EntityMap.Configure(builder);

        builder.Property(e => e.CollegeId)
            .HasColumnName("college_id")
            .IsRequired(true);

        builder.Property(e => e.ProductId)
            .HasColumnName("product_id")
            .IsRequired(true);

        builder.Property(e => e.ProvinceType)
            .HasConversion<CodeTypeValueConverter<ProvinceType>>()
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

        builder.Property(e => e.TaskCount)
            .HasColumnName("task_count")
            .IsRequired(true);

        builder.Property(e => e.TeacherCount)
            .HasColumnName("teacher_count")
            .IsRequired(true);
    }
}