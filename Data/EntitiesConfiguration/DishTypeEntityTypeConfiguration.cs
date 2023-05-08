using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class DishTypeEntityTypeConfiguration : BaseEntityTypeConfiguration<DishType>
{
    public override void Configure(EntityTypeBuilder<DishType> builder)
    {
        base.Configure(builder);
        
        builder.HasMany(x => x.Dishes)
            .WithOne(x => x.Type)
            .OnDelete(DeleteBehavior.NoAction);
    }
}