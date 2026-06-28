// File: Program.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Register our AppDbContext into the Dependency Injection container
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

// Ensure the database and tables exist before starting the server
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

Console.WriteLine("--- .NET 10 REST API Lab Running ---");

// ROUTE 1: GET all guests from MS SQL Server
app.MapGet("/api/guests", async (AppDbContext context) =>
{
    // Fetch records asynchronously from the database
    var guests = await context.Guests.ToListAsync();
    return Results.Ok(guests);
});

// ROUTE 2: POST a new guest into MS SQL Server
app.MapPost("/api/guests", async (AppDbContext context, Guest newGuest) =>
{
    if (string.IsNullOrWhiteSpace(newGuest.Name))
    {
        return Results.BadRequest("Guest name cannot be empty.");
    }

    // Add and save to database asynchronously
    context.Guests.Add(newGuest);
    await context.SaveChangesAsync();

    // Return 201 Created status code with the saved entity
    return Results.Created($"/api/guests/{newGuest.Id}", newGuest);
});

// Start the API on port 5000, listening on all network interfaces
app.Run("http://0.0.0.0:5000");
