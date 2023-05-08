using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class MenuEntityTypeConfiguration : BaseEntityTypeConfiguration<Menu>
{
    public override void Configure(EntityTypeBuilder<Menu> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => x.CreatedAt);

        builder.HasOne<Kitchen>()
            .WithMany()
            .HasForeignKey(x => x.KitchenId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(x => x.KitchenId);
    }
}