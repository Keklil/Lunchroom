using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> orderConfiguration)
    {
        orderConfiguration.HasKey(x => x.Id);

        orderConfiguration.Property(x => x.Id)
            .ValueGeneratedNever();

        orderConfiguration.Property(x => x.GroupId);

        orderConfiguration.Property(x => x.CustomerId)
            .HasField("_customerId")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        orderConfiguration.Property(x => x.LunchSetId)
            .HasField("_lunchSetId")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        orderConfiguration.Property(x => x.MenuId)
            .HasField("_menuId")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        orderConfiguration.Property(x => x.OrderDate)
            .HasField("_orderDate")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        orderConfiguration.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);

        orderConfiguration.HasOne<Menu>()
            .WithMany()
            .HasForeignKey(x => x.MenuId)
            .OnDelete(DeleteBehavior.NoAction);

        orderConfiguration.Ignore(x => x.LunchSet);
    }
}