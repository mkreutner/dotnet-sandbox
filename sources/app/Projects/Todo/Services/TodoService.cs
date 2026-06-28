// File: Services/TodoService.cs
using Microsoft.EntityFrameworkCore;

public class TodoService : ITodoService
{
  private readonly TodoDbContext _context;

  // Le constructeur reçoit le contexte de la DB automatiquement !
  public TodoService(TodoDbContext context)
  {
    _context = context;
  }
 
  // Retrieve all Todo Items
  public async Task<List<TodoItem>> GetAllTodoAsync()
  {
    return await _context.TodoItems.ToListAsync();
  }

  // Add a new todo item
  public async Task<TodoItem> AddTodoAsync(TodoItem item) 
  {
    // The title cannot be empty
    if (string.IsNullOrWhiteSpace(item.Title))
    {
      throw new ArgumentException("Todo item title cannot be empty.");
    }

    // The title must be a maximum of 100 characters long.
    if (item.Title.Length > 100)
    {
      throw new ArgumentOutOfRangeException("Todo item title must be a maximum of 100 characters long.");
    }

    await _context.TodoItems.AddAsync(item);
    await _context.SaveChangesAsync();
        
    return item; 
  }

  // Change todo item status
  public async Task<bool> CompleteTodoAsync(int id)
  {
    var item = await _context.TodoItems.FindAsync(id);

    if (item == null)
    {
      throw new KeyNotFoundException("Todo item not found.");
    }
    
    item.IsCompleted = true;
    await _context.SaveChangesAsync();
    
    return true; 
  }
}
