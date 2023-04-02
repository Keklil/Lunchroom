using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.EntitiyConfiguration;

internal class GroupSettingsEntityTypeConfiguration : IEntityTypeConfiguration<GroupKitchenSettings>
{
    public void Configure(EntityTypeBuilder<GroupKitchenSettings> groupConfiguration)
    {
        groupConfiguration.HasKey(x => x.Id);

        groupConfiguration.Property(x => x.Id)
            .ValueGeneratedNever();
    }
}