// File: Services/TodoService.cs
using Microsoft.EntityFrameworkCore;
using Todo.DTOs;

public class TodoService : ITodoService
{
  private readonly TodoDbContext _context;

  // Le constructeur reçoit le contexte de la DB automatiquement !
  public TodoService(
      TodoDbContext context
  ) {
    _context = context;
  }
 
  // Retrieve all Todo Items
  public async Task<List<TodoItem>> GetAllTodoAsync()
  {
    return await _context.TodoItems.ToListAsync();
  }

  // Add a new todo item
  public async Task<TodoResponseDto> AddTodoAsync(TodoCreateDto dto) 
  {
    // The title cannot be empty
    if (string.IsNullOrWhiteSpace(dto.Title))
    {
      throw new ArgumentException("Todo item title cannot be empty.");
    }

    // The title must be a maximum of 100 characters long.
    if (dto.Title.Length > 100)
    {
      throw new ArgumentOutOfRangeException("Todo item title must be a maximum of 100 characters long.");
    }

    // Transform dto to TodoItem entity.
    var item = dto.ToEntity();

    await _context.TodoItems.AddAsync(item);
    await _context.SaveChangesAsync();
    
    // Transform item created to TodoResponseDto
    var createdDto = item.ToResponseDto();

    return createdDto; 
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
