using Core;
using Core.CodeType;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ClientKeyEntityMap : IEntityTypeConfiguration<ClientKeyEntity>
{
    public void Configure(EntityTypeBuilder<ClientKeyEntity> builder)
    {
        builder.ToTable("tbl_client_key");

        EntityMap.Configure(builder);

        builder.Property(e => e.CollegeId)
            .HasColumnName("college_id")
            .IsRequired(true);

        builder.Property(e => e.ProductId)
            .HasColumnName("product_id")
            .IsRequired(true);

        builder.Property(e => e.KeyType)
            .HasConversion<CodeTypeConversion<ClientKeyType>>()
            .HasColumnName("key_type")
            .IsRequired(true);

        builder.Property(e => e.ClientKey)
            .HasColumnName("client_key")
            .HasMaxLength(128)
            .IsRequired(true);

        builder.Property(e => e.UserId)
            .HasColumnName("user_id")
            .IsRequired(true);
    }
}