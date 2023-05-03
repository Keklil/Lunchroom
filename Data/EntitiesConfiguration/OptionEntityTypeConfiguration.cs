using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class OptionEntityTypeConfiguration : IEntityTypeConfiguration<Option>
{
    public void Configure(EntityTypeBuilder<Option> optionConfiguraion)
    {
        optionConfiguraion.HasKey(x => x.Id);

        optionConfiguraion.Property(x => x.Id)
            .ValueGeneratedNever();

        optionConfiguraion.Property<string>(x => x.Name);

        optionConfiguraion.Property(x => x.Price);

        optionConfiguraion.HasOne(x => x.Dish);
    }
}