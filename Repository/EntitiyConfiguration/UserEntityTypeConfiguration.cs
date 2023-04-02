using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.EntitiyConfiguration;

internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> userConfiguration)
    {
        userConfiguration.HasKey(x => x.Id);

        userConfiguration.Property(x => x.Id)
            .ValueGeneratedNever();

        userConfiguration.Property(x => x.Name);

        userConfiguration.Property(x => x.Surname);

        userConfiguration.Property(x => x.Patronymic);

        userConfiguration.HasMany(x => x.Groups)
            .WithMany(x => x.Members);
    }
}