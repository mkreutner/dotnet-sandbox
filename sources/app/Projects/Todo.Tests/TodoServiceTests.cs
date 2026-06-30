// File: MyTodoApi.Tests/TodoServiceTests.cs
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;
using Todo.DTOs;
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
	public async Task AddTodoAsync_ShouldThrowArgumentException_WhenTitleIsEmpty()
	{
    var context = GetDbContextInMemory();
    var validator = new TodoCreateDtoValidator();
    var service = new TodoService(context, validator);
    
    // On passe désormais un TodoCreateDto !
    var invalidDto = new TodoCreateDto { Title = "" };

    var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => 
    {
    	await service.AddTodoAsync(invalidDto);
    });

    var error = exception.Errors.FirstOrDefault(e => e.PropertyName == "Title");
    Assert.NotNull(error);
    Assert.Equal("Todo item title cannot be empty.", error.ErrorMessage);
	}

	// 2. Test du titre trop long
	[Fact]
	public async Task AddTodoAsync_ShouldThrowArgumentOutOfRangeException_WhenTitleIsTooLong()
	{
    var context = GetDbContextInMemory();
    var validator = new TodoCreateDtoValidator();
    var service = new TodoService(context, validator);
    string longTitle = new string('A', 105); 
    
    var invalidDto = new TodoCreateDto { Title = longTitle };

    var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => 
    {
      await service.AddTodoAsync(invalidDto);
    });
    
    var error = exception.Errors.FirstOrDefault(e => e.PropertyName == "Title");
    Assert.NotNull(error);
    Assert.Equal("Todo item title must be a maximum of 100 characters long.", error.ErrorMessage);
	}

	// 3. Test du cas nominal (Happy Path)
	[Fact]
	public async Task AddTodoAsync_ShouldSaveTodoItem_WhenItemIsValid()
	{
    var context = GetDbContextInMemory();
    var validator = new TodoCreateDtoValidator();
    var service = new TodoService(context, validator);
    
    var validDto = new TodoCreateDto { Title = "Learn to use FluentValidation" };

    // Le résultat est maintenant un TodoResponseDto
    TodoResponseDto result = await service.AddTodoAsync(validDto);

    Assert.NotNull(result);
    Assert.True(result.Id > 0); 
    Assert.Equal("Learn to use FluentValidation", result.Title);
    Assert.False(result.IsCompleted);
	}
}

