using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class PassResCateEntityMap : IEntityTypeConfiguration<PassResCateEntity>
{
    public void Configure(EntityTypeBuilder<PassResCateEntity> builder)
    {
        builder.ToTable("tbl_pass_res_cate");

        EntityAssignedIdMap.Configure(builder);

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(128)
            .IsRequired(true);
    }
}