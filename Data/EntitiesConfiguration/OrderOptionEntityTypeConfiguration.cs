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

        orderOptionConfiguration.Property<int>("_optionUnits")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        orderOptionConfiguration.Property(x => x.OptionId)
            .HasField("_optionId")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}