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
// Register AutoMapper into the Dependency Injection container
builder.Services.AddAutoMapper(typeof(TodoProfile));

// Register our global handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(); // Optional but recommended in .NET 10 to standardize errors

var app = builder.Build();

// Enables the exception middleware at the very beginning of the pipeline!
app.UseExceptionHandler();

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
  // If AddTodoAsync throws an ArgumentException, the middleware catches it and returns a 400
  var createdItem = await todoService.AddTodoAsync(todoItem); 
  
  return Results.Created($"/api/todo/{createdItem.Id}", createdItem);
});


// ROUTE 3: PUT completed item
app.MapPut("/api/todo/{id}/complete", async (int id, ITodoService todoService) =>
{
  // If the ID does not exist, CompleteTodoAsync throws a KeyNotFoundException. 
  // The middleware catches it immediately and returns a 404! 
  await todoService.CompleteTodoAsync(id); 

  return Results.Ok(new { Message = $"Todo {id} marked as completed." });
});

// Start the API on port 5000, accessible from outside the container
app.Run("http://0.0.0.0:5000");
