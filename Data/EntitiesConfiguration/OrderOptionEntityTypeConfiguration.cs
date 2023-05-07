using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class OrderOptionEntityTypeConfiguration : BaseEntityTypeConfiguration<OrderOption>
{
    public override void Configure(EntityTypeBuilder<OrderOption> builder)
    {
        base.Configure(builder);
        
        builder.Property(x => x.OptionUnits);
    }
}