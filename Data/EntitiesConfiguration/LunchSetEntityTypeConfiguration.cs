using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class LunchSetEntityTypeConfiguration : IEntityTypeConfiguration<LunchSet>
{
    public void Configure(EntityTypeBuilder<LunchSet> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        builder.Property<DateTime>("CreatedAt")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("now()");

        builder.Property<DateTime>("UpdatedAt")
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("now()");
    }
}