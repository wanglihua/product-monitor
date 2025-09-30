using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class WebSessionEntityMap : IEntityTypeConfiguration<WebSessionEntity>
{
    public void Configure(EntityTypeBuilder<WebSessionEntity> builder)
    {
        builder.ToTable("tbl_web_session");

        EntityMap.Configure(builder);

        builder.Property(e => e.SessionId)
            .HasColumnName("session_id")
            .HasMaxLength(50)
            .IsRequired(true);
    }
}