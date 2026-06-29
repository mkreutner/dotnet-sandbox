// File: TodoDbContext.cs
using Microsoft.EntityFrameworkCore;

public class TodoDbContext : DbContext
{
    // Constructor which allows database in memory during Tests.
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
    }

    // This DbSet represents the Guests table in our SQL database
    public DbSet<TodoItem> TodoItems { get; set; }

    // Configuring the connection string to our MS SQL Server container
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // "database" is the name of our service in docker-compose.yml
        // We use the strong password configured earlier
        string connectionString = "Server=database;Database=TodoDb;User Id=sa;Password=D3vDotnet10;TrustServerCertificate=True;";
       
        if (!optionsBuilder.IsConfigured)
        {
          optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
