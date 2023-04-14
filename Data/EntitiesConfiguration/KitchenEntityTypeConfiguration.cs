using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class KitchenEntityTypeConfiguration : IEntityTypeConfiguration<Kitchen>
{
    public void Configure(EntityTypeBuilder<Kitchen> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.OwnsOne(x => x.Contacts);

        builder.HasMany(x => x.Managers)
            .WithMany();
    }
}