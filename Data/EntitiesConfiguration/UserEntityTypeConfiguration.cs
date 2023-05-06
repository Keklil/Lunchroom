using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> userConfiguration)
    {
        userConfiguration.HasKey(x => x.Id);

        userConfiguration.Property(x => x.Id)
            .ValueGeneratedNever();

        userConfiguration.HasMany(x => x.Groups)
            .WithMany(x => x.Members);
    }
}