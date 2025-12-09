using Core;
using Core.CodeType;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class UserProvinceEntityMap : IEntityTypeConfiguration<UserProvinceEntity>
{
    public void Configure(EntityTypeBuilder<UserProvinceEntity> builder)
    {
        builder.ToTable("tbl_user_province");

        EntityNoUpdateMap.Configure(builder);

        builder.Property(e => e.UserId)
            .HasColumnName("user_id")
            .IsRequired(true);

        builder.Property(e => e.ProvinceType)
            .HasConversion<CodeTypeConversion<ProvinceType>>()
            .HasColumnName("province_type")
            .IsRequired(true);
    }
}