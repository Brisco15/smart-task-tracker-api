using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace SmartTaskTracker.API.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            
            // Read from environment variable (for migrations)
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
            
            // Fallback to localhost for local development
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = "Host=localhost;Database=SmartTaskTrackerDB;Username=postgres;Password=postgres";
            }
            
            optionsBuilder.UseNpgsql(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
