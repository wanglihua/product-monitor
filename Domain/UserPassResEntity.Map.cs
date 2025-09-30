using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class UserPassResEntityMap : IEntityTypeConfiguration<UserPassResEntity>
{
    public void Configure(EntityTypeBuilder<UserPassResEntity> builder)
    {
        builder.ToTable("tbl_user_pass_res");

        EntityNoUpdateMap.Configure(builder);

        builder.Property(e => e.UserId)
            .HasColumnName("user_id")
            .IsRequired(true);

        builder.Property(e => e.PassResId)
            .HasColumnName("pass_res_id")
            .IsRequired(true);
    }
}