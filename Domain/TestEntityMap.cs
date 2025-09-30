using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class TestEntityMap : IEntityTypeConfiguration<TestEntity>
{
    public void Configure(EntityTypeBuilder<TestEntity> builder)
    {
        builder.ToTable("tbl_test");

        EntityMap.Configure(builder);

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(128)
            .IsRequired(true);
    }
}