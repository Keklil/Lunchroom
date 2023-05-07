using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class GroupSettingsEntityTypeConfiguration : BaseEntityTypeConfiguration<GroupSettings>
{
    public override void Configure(EntityTypeBuilder<GroupSettings> builder)
    {
        base.Configure(builder);
        
        builder.Property(x => x.Location)
            .HasColumnType("geometry (point)");
    }
}