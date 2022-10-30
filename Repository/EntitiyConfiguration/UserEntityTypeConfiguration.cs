using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;


namespace Repository.EntitiyConfiguration
{
    class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> userConfiguration)
        {                       

            userConfiguration.HasKey(x => x.Id);

            userConfiguration.Property(x => x.Id)
                .ValueGeneratedNever();

            userConfiguration.Property(x => x.Name);

            userConfiguration.Property(x => x.Surname);

            userConfiguration.Property(x => x.Patronymic);

            //userConfiguration.Property(x => x.Email);

        }
    }
}
