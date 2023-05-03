using Domain.Models;
using Domain.SecurityModels;
using Microsoft.EntityFrameworkCore;

namespace Data;

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
    public DbSet<Kitchen> Kitchens { get; set; }
    public DbSet<KitchenSettings> KitchenSettings { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<DishType> DishTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepositoryContext).Assembly);
        modelBuilder.HasPostgresExtension("postgis");
    }

    public RepositoryContext(DbContextOptions options) : base(options)
    {
        //Database.EnsureCreated();
    }
}