using Todo.DTOs;

namespace Todo.Services;

public interface ITodoService
{
    Task<TodoResponseDto> CreateAsync(TodoCreateDto dto);
    Task<IEnumerable<TodoResponseDto>> GetAllAsync();
    Task<TodoResponseDto> GetByIdAsync(int id);
    Task<TodoResponseDto> UpdateAsync(int id, TodoUpdateDto dto);
    Task DeleteAsync(int id);
}

