using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core;

public class EntityNoCreateNoUpdateMap
{
    public static void Configure<T>(EntityTypeBuilder<T> builder) where T : EntityNoCreateNoUpdate
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
    }
}