using Core;
using Core.CodeType;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ExceptionMonitorEntityMap : IEntityTypeConfiguration<ExceptionMonitorEntity>
{
    public void Configure(EntityTypeBuilder<ExceptionMonitorEntity> builder)
    {
        builder.ToTable("tbl_exception_monitor");

        EntityMap.Configure(builder);

        builder.Property(e => e.ProductId)
            .HasColumnName("product_id")
            .IsRequired(true);

        builder.Property(e => e.IpAddress)
            .HasColumnName("ip_address")
            .HasMaxLength(32)
            .IsRequired(true);

        builder.Property(e => e.ProvinceType)
            .HasConversion<CodeTypeConvertions<ProvinceType>>()
            .HasColumnName("province_type")
            .IsRequired(true);

        builder.Property(e => e.Port)
            .HasColumnName("port")
            .IsRequired(true);
    }
}