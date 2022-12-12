using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.EntitiyConfiguration
{
    class LunchSetEntityTypeConfiguration : IEntityTypeConfiguration<LunchSet>
    {
        public void Configure(EntityTypeBuilder<LunchSet> lunchSetConfiguration)
        {            
            lunchSetConfiguration.HasKey(x => x.Id);

            lunchSetConfiguration.Property(x => x.Id)
                .ValueGeneratedNever();

            lunchSetConfiguration.Property(x => x.Price);

            lunchSetConfiguration.Property(x => x.LunchSetList);

        }
    }
}
