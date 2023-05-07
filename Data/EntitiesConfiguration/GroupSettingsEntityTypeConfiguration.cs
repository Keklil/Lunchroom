using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class GroupSettingsEntityTypeConfiguration : IEntityTypeConfiguration<GroupSettings>
{
    public void Configure(EntityTypeBuilder<GroupSettings> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Location)
            .HasColumnType("geometry (point)");
        
        builder.Property<DateTime>("CreatedAt")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("now()");

        builder.Property<DateTime>("UpdatedAt")
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("now()");
    }
}