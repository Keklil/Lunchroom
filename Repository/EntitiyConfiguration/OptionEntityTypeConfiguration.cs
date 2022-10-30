using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.EntitiyConfiguration
{
    class OptionEntityTypeConfiguration : IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> optionConfiguraion)
        {
            optionConfiguraion.HasKey(x => x.Id);

            optionConfiguraion.Property(x => x.Id)
                .ValueGeneratedNever();

            optionConfiguraion.Property<string>(x => x.Name);

            optionConfiguraion.Property<decimal>(x => x.Price);
        }
    }
}
