// File: Program.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

using Todo.DTOs;
using Todo.Services;
using Todo.Models;
using Todo.Validators;
using Todo.Data;
using Todo.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Register our TodoDbContext into the Dependency Injection container
builder.Services.AddDbContext<TodoDbContext>();
// Register our TodoService into the Dependency Injection container
builder.Services.AddScoped<ITodoService, TodoService>();

// Register our global handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(); // Optional but recommended in .NET 10 to standardize errors
builder.Services.AddValidatorsFromAssemblyContaining<TodoCreateDtoValidator>();

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
    var todoItems = await todoService.GetAllAsync();
    return Results.Ok(todoItems);
});

// ROUTE 2: POST a new todo item into MS SQL Server
app.MapPost("/api/todo", async (TodoCreateDto dto, ITodoService todoService) =>
{
  // If AddTodoAsync throws an ArgumentException, the middleware catches it and returns a 400
  var responseDto = await todoService.CreateAsync(dto); 
  
  return Results.Created($"/api/todo/{responseDto.Id}", responseDto);
});

// Start the API on port 5000, accessible from outside the container
app.Run("http://0.0.0.0:5000");
