using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core;

public static class EntityAssignedIdNoCreateNoUpdateMap
{
    public static void Configure<T>(EntityTypeBuilder<T> builder) where T : EntityAssignedIdNoCreateNoUpdate
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
    }
}