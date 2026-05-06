using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SmartTaskTracker.API.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // ✅ Changed from UseSqlServer to UseNpgsql for PostgreSQL
            optionsBuilder.UseNpgsql("Host=localhost;Database=SmartTaskTrackerDB;Username=postgres;Password=postgres");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
