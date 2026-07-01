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
    throw new NotImplementedException();
  }

  public async Task<TodoResponseDto> GetByIdAsync(int id)
  {
    throw new NotImplementedException();
  }

  public async Task<TodoResponseDto> UpdateAsync(int id, TodoUpdateDto dto)
  {
    throw new NotImplementedException();
  }

  public async Task DeleteAsync(int id)
  {
    throw new NotImplementedException();
  }
}
