// File: Projects/Todo/DTOs/TodoDtos.cs
namespace Todo.DTOs;

public class TodoCreateDto
{
    // Le client ne fournit QUE le titre à la création.
    public string Title { get; set; } = string.Empty;
}

public class TodoResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}

