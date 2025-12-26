using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProductInstallAttachmentEntityMap : IEntityTypeConfiguration<ProductInstallAttachmentEntity>
{
    public void Configure(EntityTypeBuilder<ProductInstallAttachmentEntity> builder)
    {
        builder.ToTable("tbl_product_install_attachment");

        EntityMap.Configure(builder);

        builder.Property(e => e.ProductInstallId)
            .HasColumnName("product_install_id")
            .IsRequired(true);

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired(true);

        builder.Property(e => e.PathFile)
            .HasColumnName("path_file")
            .HasMaxLength(300)
            .IsRequired(true);
    }
}