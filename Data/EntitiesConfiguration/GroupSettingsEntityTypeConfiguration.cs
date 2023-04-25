using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class GroupSettingsEntityTypeConfiguration : IEntityTypeConfiguration<GroupSettings>
{
    public void Configure(EntityTypeBuilder<GroupSettings> groupConfiguration)
    {
        groupConfiguration.HasKey(x => x.Id);

        groupConfiguration.Property(x => x.Id)
            .ValueGeneratedNever();

        groupConfiguration.Property(x => x.Location)
            .HasColumnType("geometry (point)");
    }
}