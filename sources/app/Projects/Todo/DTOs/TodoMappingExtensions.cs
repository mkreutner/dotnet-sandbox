// File: Projects/Todo/DTOs/TodoMappingExtensions.cs
using Todo.DTOs;

public static class TodoMappingExtensions
{
    // Convertit un TodoCreateDto en entité TodoItem (pour l'insertion)
    public static TodoItem ToEntity(this TodoCreateDto dto)
    {
        return new TodoItem
        {
            Title = dto.Title,
            IsCompleted = false // Valeur par défaut à la création
        };
    }

    // Convertit une entité TodoItem en TodoResponseDto (pour la sortie API)
    public static TodoResponseDto ToResponseDto(this TodoItem item)
    {
        return new TodoResponseDto
        {
            Id = item.Id,
            Title = item.Title,
            IsCompleted = item.IsCompleted
        };
    }
}
