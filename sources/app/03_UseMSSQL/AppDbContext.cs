// File: AppDbContext.cs
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    // This DbSet represents the Guests table in our SQL database
    public DbSet<Guest> Guests { get; set; }

    // Configuring the connection string to our MS SQL Server container
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // "database" is the name of our service in docker-compose.yml
        // We use the strong password configured earlier
        string connectionString = "Server=database;Database=GuestDb;User Id=sa;Password=D3vDotnet10;TrustServerCertificate=True;";
        
        optionsBuilder.UseSqlServer(connectionString);
    }
}
