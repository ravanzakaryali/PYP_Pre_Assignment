using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Data.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Segment> Segments { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<SaleTransaction> SaleTransactions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
