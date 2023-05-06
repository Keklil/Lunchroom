using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class MenuEntityTypeConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> menuConfiguration)
    {
        menuConfiguration.HasKey(x => x.Id);

        menuConfiguration.Property(x => x.Id)
            .ValueGeneratedNever();
        
        menuConfiguration.Property(x => x.Date);
    }
}