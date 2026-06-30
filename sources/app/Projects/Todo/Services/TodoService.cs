// File: Services/TodoService.cs
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Todo.DTOs;

public class TodoService : ITodoService
{
  private readonly TodoDbContext _context;
  private readonly IValidator<TodoCreateDto> _validator;

  // Le constructeur reçoit le contexte de la DB automatiquement !
  public TodoService(
      TodoDbContext context,
      IValidator<TodoCreateDto> validator
  ) {
    _context = context;
    _validator = validator;
  }
 
  // Retrieve all Todo Items
  public async Task<List<TodoItem>> GetAllTodoAsync()
  {
    return await _context.TodoItems.ToListAsync();
  }

  // Add a new todo item
  public async Task<TodoResponseDto> AddTodoAsync(TodoCreateDto dto) 
  {
    // The title validation
    var validationResult = await _validator.ValidateAsync(dto);

    // DTO is not valid
    if (!validationResult.IsValid)
    {
      throw new FluentValidation.ValidationException(validationResult.Errors);
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
