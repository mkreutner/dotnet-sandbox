// File: MyTodoApi.Tests/TodoServiceTests.cs
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;
using Todo.DTOs;

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
    var service = new TodoService(context);
    
    // On passe désormais un TodoCreateDto !
    var invalidDto = new TodoCreateDto { Title = "" };

    await Assert.ThrowsAsync<ArgumentException>(async () => 
    {
    	await service.AddTodoAsync(invalidDto);
    });
	}

	// 2. Test du titre trop long
	[Fact]
	public async Task AddTodoAsync_ShouldThrowArgumentOutOfRangeException_WhenTitleIsTooLong()
	{
    var context = GetDbContextInMemory();
    var service = new TodoService(context);
    string longTitle = new string('A', 105); 
    
    var invalidDto = new TodoCreateDto { Title = longTitle };

    await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => 
    {
      await service.AddTodoAsync(invalidDto);
    });
	}

	// 3. Test du cas nominal (Happy Path)
	[Fact]
	public async Task AddTodoAsync_ShouldSaveTodoItem_WhenItemIsValid()
	{
    var context = GetDbContextInMemory();
    var service = new TodoService(context);
    
    var validDto = new TodoCreateDto { Title = "Acheter du café" };

    // Le résultat est maintenant un TodoResponseDto
    TodoResponseDto result = await service.AddTodoAsync(validDto);

    Assert.NotNull(result);
    Assert.True(result.Id > 0); 
    Assert.Equal("Acheter du café", result.Title);
    Assert.False(result.IsCompleted);
	}
}

