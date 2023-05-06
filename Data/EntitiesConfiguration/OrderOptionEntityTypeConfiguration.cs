using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class OrderOptionEntityTypeConfiguration : IEntityTypeConfiguration<OrderOption>
{
    public void Configure(EntityTypeBuilder<OrderOption> orderOptionConfiguration)
    {
        orderOptionConfiguration.HasKey(x => x.Id);

        orderOptionConfiguration.Property(x => x.Id)
            .ValueGeneratedNever();
    }
}