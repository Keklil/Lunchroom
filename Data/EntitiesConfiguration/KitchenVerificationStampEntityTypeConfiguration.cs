using Domain.Infrastructure;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class KitchenVerificationStampEntityTypeConfiguration : IEntityTypeConfiguration<KitchenVerificationStamp>
{
    public void Configure(EntityTypeBuilder<KitchenVerificationStamp> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.VerificationTimestamp);

        builder.HasOne(x => x.Checker)
            .WithMany();
    }
}