using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class KitchenSettingsEntityTypeConfiguration : IEntityTypeConfiguration<KitchenSettings>
{
    public void Configure(EntityTypeBuilder<KitchenSettings> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.OwnsMany(x => x.ShippingAreas).Property(x => x.Polygon).HasColumnType("geometry (polygon)");
    }
}