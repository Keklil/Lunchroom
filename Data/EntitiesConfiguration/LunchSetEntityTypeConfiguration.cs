using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class LunchSetEntityTypeConfiguration : IEntityTypeConfiguration<LunchSet>
{
    public void Configure(EntityTypeBuilder<LunchSet> lunchSetConfiguration)
    {
        lunchSetConfiguration.HasKey(x => x.Id);

        lunchSetConfiguration.Property(x => x.Id)
            .ValueGeneratedNever();
    }
}