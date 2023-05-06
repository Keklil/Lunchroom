using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Domain.Infrastructure;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration;

internal class GroupEntityTypeConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.HasOne<User>(x => x.Admin)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne<Kitchen>()
            .WithMany()
            .HasForeignKey(x => x.SelectedKitchenId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.OwnsOne(x => x.PaymentInfo, m =>
        {
            m.Property(p => p.Description);
            m.Property(p => p.Qr);
            m.Property(p => p.Link);
            m.ToTable(nameof(PaymentInfo));
        });

        builder.Property(x => x.Referral)
            .HasConversion(x => JsonSerializer.Serialize(x, JsonSerializerOptions.Default),
                x => JsonSerializer.Deserialize<GroupReferral>(x, JsonSerializerOptions.Default)!);
    }
}