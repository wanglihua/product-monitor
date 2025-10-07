using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ActionLogEntityMap : IEntityTypeConfiguration<ActionLogEntity>
{
    public void Configure(EntityTypeBuilder<ActionLogEntity> builder)
    {
        builder.ToTable("tbl_action_log");

        EntityNoUpdateMap.Configure(builder);

        builder.Property(e => e.Type)
            .HasConversion<ActionLogTypeValueConverter>()
            .HasColumnName("type_code")
            .IsRequired(true);

        builder.Property(e => e.UserId)
            .HasColumnName("user_id")
            .IsRequired(true);

        builder.Property(e => e.ActionDesc)
            .HasColumnName("action_desc")
            .IsRequired(true);
    }
}