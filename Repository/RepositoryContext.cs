using Domain.Models;
using Domain.SecurityModels;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class RepositoryContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Menu> Menu { get; set; }
    public DbSet<LunchSet> LunchSets { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderOption> OrdersOptions { get; set; }
    public DbSet<EmailValidation> EmailValidations { get; set; }
    public DbSet<Group> Groups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepositoryContext).Assembly);
    }

    public RepositoryContext(DbContextOptions options) : base(options)
    {
        //Database.EnsureCreated();
    }
}