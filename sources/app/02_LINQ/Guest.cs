// File: Guest.cs
public class Guest
{
    // Properties must have public getters and setters for the JSON serializer
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }

    // Default constructor needed for JSON deserialization
    public Guest()
    {
    }

    // Custom constructor for easy creation
    public Guest(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"- {Name} ({Age} years old)");
    }
}
