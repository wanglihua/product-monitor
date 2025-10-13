using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core;

public static class EntityAssignedIdMap
{
    public static void Configure<T>(EntityTypeBuilder<T> builder) where T : EntityAssignedId
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.CreateTime).HasColumnName("create_time");
        builder.Property(e => e.UpdateTime).HasColumnName("update_time");
    }
}