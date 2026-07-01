// File: Services/TodoService.cs
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Todo.DTOs;
using Todo.Data;

namespace Todo.Services;

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
 
  // Add a new todo item
  public async Task<TodoResponseDto> CreateAsync(TodoCreateDto dto) 
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

  public async Task<IEnumerable<TodoResponseDto>> GetAllAsync()
  {
    var items = await _context.TodoItems.ToListAsync();
    return items.ToResponseDtoList();
  }

  public async Task<TodoResponseDto> GetByIdAsync(int id)
  {
    var item = await _context.TodoItems.FindAsync(id);

    if (item is null)
    {
      throw new KeyNotFoundException($"Todo item with id {id} was not found.");
    }

    return item.ToResponseDto();
  }

  public async Task<TodoResponseDto> UpdateAsync(int id, TodoUpdateDto dto)
  {
    var item = await _context.TodoItems.FindAsync(id);

    if (item is null)
    {
      throw new KeyNotFoundException($"Todo item with id {id} was not found.");
    }

    item.UpdateFromDto(dto);
    await _context.SaveChangesAsync();

    return item.ToResponseDto();
  }

  public async Task DeleteAsync(int id)
  {
    var item = await _context.TodoItems.FindAsync(id);

    if (item is null)
    {
      throw new KeyNotFoundException($"Todo item with id {id} was not found.");
    }

    _context.TodoItems.Remove(item);
    await _context.SaveChangesAsync();
  }
}
