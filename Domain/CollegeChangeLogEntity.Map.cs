using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class CollegeChangeLogEntityMap : IEntityTypeConfiguration<CollegeChangeLogEntity>
{
    public void Configure(EntityTypeBuilder<CollegeChangeLogEntity> builder)
    {
        builder.ToTable("tbl_college_change_log");

        EntityNoUpdateMap.Configure(builder);

        builder.Property(e => e.CollegeId)
            .HasColumnName("college_id")
            .IsRequired(true);

        builder.Property(e => e.ChangeDesc)
            .HasColumnName("change_desc")
            .HasMaxLength(512)
            .IsRequired(true);
    }
}
