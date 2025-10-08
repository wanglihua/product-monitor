using System.ComponentModel;
using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProductVersionEntityMap : IEntityTypeConfiguration<ProductVersionEntity>
{
    public void Configure(EntityTypeBuilder<ProductVersionEntity> builder)
    {
        builder.ToTable("tbl_product_version");

        EntityMap.Configure(builder);

        builder.Property(e => e.ProductId)
            .HasColumnName("product_id")
            .IsRequired(true);

        builder.Property(e => e.Major)
            .HasColumnName("major")
            .IsRequired(true);

        builder.Property(e => e.Minor)
            .HasColumnName("minor")
            .IsRequired(true);

        builder.Property(e => e.Revision)
            .HasColumnName("revision")
            .IsRequired(true);

        builder.Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(256)
            .IsRequired(true);
    }
}