using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class UserEntityTypeConfiguration : BaseEntityTypeConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.HasMany(x => x.Groups)
            .WithMany(x => x.Members);
    }
}