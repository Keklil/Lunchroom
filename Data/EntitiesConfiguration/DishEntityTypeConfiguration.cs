using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class DishEntityTypeConfiguration : BaseEntityTypeConfiguration<Dish>
{
    public override void Configure(EntityTypeBuilder<Dish> builder)
    {
        base.Configure(builder);
    }
}