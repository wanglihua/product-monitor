using Core;
using Core.CodeType;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class WebSessionDataEntityMap : IEntityTypeConfiguration<WebSessionDataEntity>
{
    public void Configure(EntityTypeBuilder<WebSessionDataEntity> builder)
    {
        builder.ToTable("tbl_web_session_data");

        EntityMap.Configure(builder);

        builder.Property(e => e.WebSessionId)
            .HasColumnName("web_session_id")
            .IsRequired(true);

        builder.Property(e => e.SessionId)
            .HasColumnName("session_id")
            .HasMaxLength(64)
            .IsRequired(true);

        builder.Property(e => e.Type)
            .HasConversion<CodeTypeConversion<WebSessionDataType>>()
            .HasColumnName("type_code")
            .IsRequired(true);

        builder.Property(e => e.Data)
            .HasColumnName("data")
            .HasMaxLength(256)
            .IsRequired(true);
    }
}