using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FutureQueryWithConcat.Mappings;
using FutureQueryWithConcat.Models;
using Microsoft.Extensions.Logging.Debug;

namespace FutureQueryWithConcat.DbContext
{
    public class FutureDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private static readonly LoggerFactory MyLoggerFactory = new LoggerFactory(new[] { new DebugLoggerProvider() });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseLoggerFactory(MyLoggerFactory).UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FutureDbTest;Integrated Security=True;App=EntityFramework");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new TenantMapping());
            modelBuilder.ApplyConfiguration(new UserMapping());
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
