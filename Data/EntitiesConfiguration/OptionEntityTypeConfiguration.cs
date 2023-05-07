using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class OptionEntityTypeConfiguration : BaseEntityTypeConfiguration<Option>
{
    public override void Configure(EntityTypeBuilder<Option> builder)
    {
        base.Configure(builder);
        
        builder.Property(x => x.Price);
        
        builder.HasOne(x => x.Dish);

        builder.Property(x => x.Name)
            .HasField("_name")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}