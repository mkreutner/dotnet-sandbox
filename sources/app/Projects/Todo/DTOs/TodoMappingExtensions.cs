using Todo.Models;

namespace Todo.DTOs;

public static class TodoMappingExtensions
{
    // Convert TodoCreateDto int TodoItem (for INSERT)
    public static TodoItem ToEntity(this TodoCreateDto dto)
    {
        return new TodoItem
        {
            Title = dto.Title,
            IsCompleted = false // default value 
        };
    }

    // Convert TodoItem into TodoResponseDto using record constructor
    public static TodoResponseDto ToResponseDto(this TodoItem item)
    {
        // Use directly record constructor arguments
        return new TodoResponseDto(item.Id, item.Title, item.IsCompleted);
    }
    
    // Used by GET All
    public static IEnumerable<TodoResponseDto> ToResponseDtoList(this IEnumerable<TodoItem> entities)
    {
        return entities.Select(e => e.ToResponseDto());
    }

    // Apply changes of Update DTO on existing entity
    public static void UpdateFromDto(this TodoItem entity, TodoUpdateDto dto)
    {
        entity.Title = dto.Title;
        entity.IsCompleted = dto.IsCompleted;
    }
}
