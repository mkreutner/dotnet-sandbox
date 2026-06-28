// File: Program.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Register our TodoDbContext into the Dependency Injection container
builder.Services.AddDbContext<TodoDbContext>();
// Register our TodoService into the Dependency Injection container
builder.Services.AddScoped<ITodoService, TodoService>();

var app = builder.Build();

// Ensure the database exists and contains tables
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    dbContext.Database.EnsureCreated();
}

Console.WriteLine("--- Todo REST API ---");

// ROUTE 1: GET all todo items from MS SQL Server
app.MapGet("/api/todo", async (ITodoService todoService) =>
{
    var todoItems = await todoService.GetAllTodoAsync();
    return Results.Ok(todoItems);
});

// ROUTE 2: POST a new todo item into MS SQL Server
app.MapPost("/api/todo", async (TodoItem todoItem, ITodoService todoService) =>
{
  try
  {
    var createdItem = await todoService.AddTodoAsync(todoItem);
    return Results.Created($"/api/todo/{createdItem.Id}", createdItem);
  }
  catch (ArgumentException ex)
  {
    return Results.BadRequest(new { Error = ex.Message });
  }
  catch (Exception ex)
  {
    return Results.InternalServerError(new { Error = "An unexpected error occurred." });
  }
});


// ROUTE 3: PUT completed item
app.MapPut("/api/todo/{id}/complete", async (int id, ITodoService todoService) =>
{
    try 
    {
      await todoService.CompleteTodoAsync(id);
      return Results.Ok(new { Message = $"Todo {id} marked as completed." });
    }
    catch (KeyNotFoundException ex)
    {
      return Results.NotFound(new { Error = $"Todo {id} not found." });
    }
    catch (Exception ex)
    {
      return Results.InternalServerError(new { Error = "An unexpected error occured." });
    }
});

// Start the API on port 5000, accessible from outside the container
app.Run("http://0.0.0.0:5000");
