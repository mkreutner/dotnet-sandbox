// File: Guest.cs
public class Guest
{
    // EF Core will automatically recognize this property as the Primary Key
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }

    // Default constructor for EF Core / JSON
    public Guest()
    {
    }

    // Updated constructor
    public Guest(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public void DisplayInfo()
    {
        // Added the Id to the display to see EF Core in action
        Console.WriteLine($"[{Id}] - {Name} ({Age} years old)");
    }
}
