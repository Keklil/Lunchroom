using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.EntitiyConfiguration
{
    class GroupEntityTypeConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> orderConfiguration)
        {
            orderConfiguration.HasKey(x => x.Id);

            orderConfiguration.Property(x => x.Id)
                .ValueGeneratedNever();

            orderConfiguration.HasOne<User>(x => x.Admin)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
