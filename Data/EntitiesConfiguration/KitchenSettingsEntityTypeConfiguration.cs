using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class KitchenSettingsEntityTypeConfiguration : BaseEntityTypeConfiguration<KitchenSettings>
{
    public override void Configure(EntityTypeBuilder<KitchenSettings> builder)
    {
        base.Configure(builder);
        
        builder.OwnsMany(x => x.ShippingAreas)
            .Property(x => x.Polygon)
            .HasColumnType("geometry (polygon)");
    }
}