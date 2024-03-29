﻿using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Repository.EntitiyConfiguration;

public class DbInitializer
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private string adminEmail;

    public DbInitializer(IServiceScopeFactory scopeFactory, IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;

        adminEmail = _configuration.GetSection("AdminEmail").Value;
    }

    public void Initialize()
    {
        using (var serviceScope = _scopeFactory.CreateScope())
        {
            using (var context = serviceScope.ServiceProvider.GetService<RepositoryContext>())
            {
                context.Database.Migrate();
            }
        }
    }

    public void SeedData()
    {
        using (var serviceScope = _scopeFactory.CreateScope())
        {
            using (var context = serviceScope.ServiceProvider.GetService<RepositoryContext>())
            {
                if (!context.Users.Any())
                {
                    var adminUser = new User(adminEmail, "asdfg123", true);
                    adminUser.CheckEmail();
                    context.Users.Add(adminUser);
                }

                context.SaveChanges();
            }
        }
    }
}
