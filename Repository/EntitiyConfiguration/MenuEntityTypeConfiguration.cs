using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.EntitiyConfiguration;

internal class MenuEntityTypeConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> menuConfiguration)
    {
        menuConfiguration.HasKey(x => x.Id);

        menuConfiguration.Property(x => x.Id)
            .ValueGeneratedNever();

        /*menuConfiguration
            .Property<DateTime>("_date")
            .UsePropertyAccessMode(PropertyAccessMode.Field);  
        */
        menuConfiguration.Property(x => x.Date);

        menuConfiguration.Property(x => x.GroupId);
    }
}