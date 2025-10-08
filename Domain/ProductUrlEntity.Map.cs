using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProductUrlEntityMap : IEntityTypeConfiguration<ProductUrlEntity>
{
    public void Configure(EntityTypeBuilder<ProductUrlEntity> builder)
    {
        throw new NotImplementedException();
    }
}