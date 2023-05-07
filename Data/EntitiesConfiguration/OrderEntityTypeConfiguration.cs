using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class OrderEntityTypeConfiguration : BaseEntityTypeConfiguration<Order>
{
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        base.Configure(builder);
        
        builder.Property(x => x.GroupId);
        
        builder.Property(x => x.CreatedAt);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Menu>()
            .WithMany()
            .HasForeignKey(x => x.MenuId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}