using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SmartTaskTracker.API.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        // Use SQL Server for production (matching your schema)
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=smarttasktracker;Trusted_Connection=True;");

        return new AppDbContext(optionsBuilder.Options);
    }
}