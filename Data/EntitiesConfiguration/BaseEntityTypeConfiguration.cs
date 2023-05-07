using Domain.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

public class BaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : Entity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        builder.Ignore(x => x.DomainEvents);
        
        builder.Property<DateTime>("CreatedAt")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("now()");

        builder.Property<DateTime>("UpdatedAt")
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("now()");
    }
}