// File: MyTodoApi.Tests/TodoServiceTests.cs
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;
using Todo.Services;
using Todo.Models;
using Todo.DTOs;
using Todo.Validators;
using Todo.Data;
using FluentValidation;

public class TodoServiceTests
{
  private TodoDbContext GetDbContextInMemory()
  {
  	// Crée des options pour utiliser une base de données unique en mémoire
    var options = new DbContextOptionsBuilder<TodoDbContext>()
    	.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
      .Options;

    return new TodoDbContext(options);
  }

	// 1. Test du titre vide
	[Fact]
	public async Task CreateAsync_ShouldThrowArgumentException_WhenTitleIsEmpty()
	{
    var context = GetDbContextInMemory();
    var validator = new TodoCreateDtoValidator();
    var service = new TodoService(context, validator);
    
    // On passe désormais un TodoCreateDto !
    var invalidDto = new TodoCreateDto("");

    var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => 
    {
    	await service.CreateAsync(invalidDto);
    });

    var error = exception.Errors.FirstOrDefault(e => e.PropertyName == "Title");
    Assert.NotNull(error);
    Assert.Equal("Todo item title cannot be empty.", error.ErrorMessage);
	}

	// 2. Test du titre trop long
	[Fact]
	public async Task CreateAsync_ShouldThrowArgumentOutOfRangeException_WhenTitleIsTooLong()
	{
    var context = GetDbContextInMemory();
    var validator = new TodoCreateDtoValidator();
    var service = new TodoService(context, validator);
    string longTitle = new string('A', 105); 
    
    var invalidDto = new TodoCreateDto(longTitle);

    var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => 
    {
      await service.CreateAsync(invalidDto);
    });
    
    var error = exception.Errors.FirstOrDefault(e => e.PropertyName == "Title");
    Assert.NotNull(error);
    Assert.Equal("Todo item title must be a maximum of 100 characters long.", error.ErrorMessage);
	}

	// 3. Test du cas nominal (Happy Path)
	[Fact]
	public async Task CreateAsync_ShouldSaveTodoItem_WhenItemIsValid()
	{
    var context = GetDbContextInMemory();
    var validator = new TodoCreateDtoValidator();
    var service = new TodoService(context, validator);
    
    var validDto = new TodoCreateDto("Learn to use FluentValidation");

     // Le résultat est maintenant un TodoResponseDto
    TodoResponseDto result = await service.CreateAsync(validDto);

    Assert.NotNull(result);
    Assert.True(result.Id > 0); 
    Assert.Equal("Learn to use FluentValidation", result.Title);
    Assert.False(result.IsCompleted);
	}

  [Fact]
public async Task GetByIdAsync_ShouldReturnResponseDto_WhenTodoExists()
{
    // Arrange
    var context = GetDbContextInMemory();
    var validator = new Todo.Validators.TodoCreateDtoValidator(); // Instanciation explicite
    var service = new TodoService(context, validator);
    
    var existingTodo = new TodoItem { Id = 10, Title = "Acheter du pain", IsCompleted = false };
    await context.TodoItems.AddAsync(existingTodo);
    await context.SaveChangesAsync();

    // Act
    var result = await service.GetByIdAsync(10);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("Acheter du pain", result.Title);
    Assert.False(result.IsCompleted);
}

[Fact]
public async Task GetByIdAsync_ShouldThrowKeyNotFoundException_WhenTodoDoesNotExist()
{
    // Arrange
    var context = GetDbContextInMemory();
    var validator = new TodoCreateDtoValidator();
    var service = new TodoService(context, validator);

    // Act & Assert
    await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetByIdAsync(999));
}

[Fact]
public async Task UpdateAsync_ShouldModifyExistingTodo_WhenDataIsValid()
{
    // Arrange
    var context = GetDbContextInMemory();
    var validator = new TodoCreateDtoValidator();
    var service = new TodoService(context, validator);
    
    var existingTodo = new TodoItem { Id = 20, Title = "Ancien Titre", IsCompleted = false };
    await context.TodoItems.AddAsync(existingTodo);
    await context.SaveChangesAsync();

    var updateDto = new Todo.DTOs.TodoUpdateDto("Nouveau Titre", true);

    // Act
    var result = await service.UpdateAsync(20, updateDto);

    // Assert
    Assert.Equal("Nouveau Titre", result.Title);
    Assert.True(result.IsCompleted);
}

[Fact]
public async Task DeleteAsync_ShouldRemoveTodo_WhenTodoExists()
{
    // Arrange
    var context = GetDbContextInMemory();
    var validator = new TodoCreateDtoValidator();
    var service = new TodoService(context, validator);
    
    var existingTodo = new TodoItem { Id = 30, Title = "À supprimer", IsCompleted = false };
    await context.TodoItems.AddAsync(existingTodo);
    await context.SaveChangesAsync();

    // Act
    await service.DeleteAsync(30);

    // Assert
    var dbTodo = await context.TodoItems.FindAsync(30);
    Assert.Null(dbTodo);
}
}

