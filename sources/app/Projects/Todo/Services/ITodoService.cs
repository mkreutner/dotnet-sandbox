using Todo.DTOs;

// File: Services/ITodoService.cs
public interface ITodoService
{
    // Retrieve all Todo Items
    Task<List<TodoItem>> GetAllTodoAsync();

    // Add a new todo item
    Task<TodoResponseDto> AddTodoAsync(TodoCreateDto dto);

    // Change todo item status
    Task<bool> CompleteTodoAsync(int id);
}
