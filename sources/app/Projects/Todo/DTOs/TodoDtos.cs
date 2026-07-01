namespace Todo.DTOs;

// Creating Todo
public record TodoCreateDto(string Title);

// Update Todo
public record TodoUpdateDto(string Title, bool IsCompleted);

// Send to API cunsumer
public record TodoResponseDto(int Id, string Title, bool IsCompleted);
