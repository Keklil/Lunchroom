using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.EntitiyConfiguration
{
    class GroupEntityTypeConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> groupConfiguration)
        {
            groupConfiguration.HasKey(x => x.Id);

            groupConfiguration.Property(x => x.Id)
                .ValueGeneratedNever();

            groupConfiguration.HasOne<User>(x => x.Admin)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
            
            groupConfiguration.Property(x => x.Referral)
                .HasColumnType("jsonb");
        }
    }
}
