using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SmartTaskTracker.API.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Use PostgreSQL (Npgsql extension is automatically available)
            optionsBuilder.UseNpgsql("Host=localhost;Database=SmartTaskTrackerDB;Username=postgres;Password=postgres");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
