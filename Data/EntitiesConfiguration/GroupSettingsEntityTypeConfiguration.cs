using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class GroupSettingsEntityTypeConfiguration : IEntityTypeConfiguration<GroupKitchenSettings>
{
    public void Configure(EntityTypeBuilder<GroupKitchenSettings> groupConfiguration)
    {
        groupConfiguration.HasKey(x => x.Id);

        groupConfiguration.Property(x => x.Id)
            .ValueGeneratedNever();
    }
}