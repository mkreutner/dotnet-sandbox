// File: TodoItem.cs
public class TodoItem
{
    // EF Core will automatically recognize this property as the Primary Key
    public int Id { get; set; }
    
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted  { get; set; }

    // Default constructor for EF Core / JSON
    public TodoItem()
    {
    }

    // Updated constructor
    public TodoItem(string title, bool isCompleted)
    {
        Title = title;
        IsCompleted = isCompleted;
    }

    public void DisplayInfo()
    {
        // Added the Id to the display to see EF Core in action
        string status = IsCompleted ? "Completed" : "Pending";
        Console.WriteLine($"[{Id}] - {Title} ({status})");
    }
}
