// File: MyTodoApi.Tests/TodoServiceTests.cs
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

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

    [Fact] // Dit à xUnit que c'est une méthode de test
    public async Task AddTodoAsync_ShouldThrowArgumentException_WhenTitleIsEmpty()
    {
      // ARRANGE
      var context = GetDbContextInMemory();
      var service = new TodoService(context); // On injecte notre fausse DB
       
      var invalidItem = new TodoItem 
      { 
        Title = "" // Titre vide invalide !
      };

      // ACT & ASSERT
      // On demande à xUnit de vérifier que la méthode lève bien une ArgumentException
      var exception = await Assert.ThrowsAsync<ArgumentException>(async () => 
      {
        await service.AddTodoAsync(invalidItem);
      });

      // On vérifie que le message d'erreur est exactement celui que tu as écrit hier
      Assert.Equal("Todo item title cannot be empty.", exception.Message);
    }

    [Fact]
    public async Task AddTodoAsync_ShouldThrowArgumentException_WhenTitleIsTooLong()
    {
      // ARRANGE
      var context = GetDbContextInMemory();
      var service = new TodoService(context);

      // Initialize a dumy very long title than more 100 caracters
      string longTitle = new string('Q', 105);

      var invalidItem = new TodoItem
      {
        Title = longTitle,
        IsCompleted = false
      };

      // ACT & ASSERT
      var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => 
      {
        await service.AddTodoAsync(invalidItem);
      });

      Assert.Contains("Todo item title must be a maximum of 100 characters long.", exception.Message);
    }

    [Fact]
    public async Task AddTodoAsync_ShouldSaveTodoItem_WhenItemIsValid()
    {
      // ARRANGE
      var context = GetDbContextInMemory();
      var service = new TodoService(context);
    
      var validItem = new TodoItem 
      { 
        Title = "Acheter du café",
        IsCompleted = false
      };

      // ACT
      var result = await service.AddTodoAsync(validItem);

      // ASSERT
      // 1. On vérifie que le service nous renvoie bien l'objet
      Assert.NotNull(result);
    
      // 2. L'InMemoryDatabase simule l'auto-incrément des IDs, l'id doit être généré (ex: 1)
      Assert.True(result.Id > 0); 
    
      // 3. On va vérifier directement dans la base de données si l'item y est bien
      var itemInDb = await context.TodoItems.FindAsync(result.Id);
      Assert.NotNull(itemInDb);
      Assert.Equal("Acheter du café", itemInDb.Title);
    }
}
