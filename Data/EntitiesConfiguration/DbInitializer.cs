using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data.EntitiesConfiguration;

public class DbInitializer
{
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly string adminEmail;

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
                    var adminUser = User.CreateAdmin(adminEmail, "asdfg123");
                    adminUser.CheckEmail();
                    context.Users.Add(adminUser);
                }

                context.SaveChanges();
            }
        }
    }

    public DbInitializer(IServiceScopeFactory scopeFactory, IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;

        adminEmail = _configuration.GetSection("AdminEmail").Value;
    }
}