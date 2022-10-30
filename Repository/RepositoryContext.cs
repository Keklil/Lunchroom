using Entities.SecurityModels;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.EntitiyConfiguration;

namespace Repository
{
    public class RepositoryContext : DbContext
    {

        public RepositoryContext(DbContextOptions options) : base(options)
        {
            //Database.EnsureCreated();
        }
        public DbSet<User>? Users { get; set; }
        public DbSet<Menu>? Menu { get; set; }
        public DbSet<LunchSet>? LunchSets { get; set; }
        public DbSet<Option>? Options { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<OrderOption>? OrdersOptions { get; set; }
        public DbSet<EmailValidation>? EmailValidations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepositoryContext).Assembly);
        }
    }
}