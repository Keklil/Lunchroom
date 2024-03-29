﻿using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Domain.Infrastructure;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.EntitiyConfiguration
{
    class GroupEntityTypeConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> groupConfiguration)
        {
            var options = new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            }; 
            
            groupConfiguration.HasKey(x => x.Id);

            groupConfiguration.Property(x => x.Id)
                .ValueGeneratedNever();

            groupConfiguration.HasOne<User>(x => x.Admin)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            groupConfiguration.Property(x => x.Referral)
                .HasConversion(x => JsonSerializer.Serialize(x, JsonSerializerOptions.Default),
                    x => JsonSerializer.Deserialize<GroupReferral>(x, JsonSerializerOptions.Default));
        }
    }
}
