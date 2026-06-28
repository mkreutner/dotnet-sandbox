// File: Program.cs
using System.Text.Json;

// Define the file path where data will be stored inside the container
string filePath = "guests.json";
List<Guest> guestsList;

Console.WriteLine("--- .NET 10 File System Lab ---");

// Check if the backup file already exists
if (File.Exists(filePath))
{
    Console.WriteLine("Found existing data file. Loading guests...");
    
    // Read the entire text from the file
    string jsonString = File.ReadAllText(filePath);
    
    // Convert the JSON string back into a List of Guest objects
    guestsList = JsonSerializer.Deserialize<List<Guest>>(jsonString) ?? new List<Guest>();
}
else
{
    Console.WriteLine("No data file found. Initializing default layout...");
    
    // Create a default list if no file exists
    guestsList = new List<Guest>
    {
        new Guest("Alice", 28),
        new Guest("Bob", 34),
        new Guest("Charlie", 22)
    };

    // Serialize the list into a pretty-printed JSON string
    var options = new JsonSerializerOptions { WriteIndented = true };
    string jsonString = JsonSerializer.Serialize(guestsList, options);
    
    // Write the string into the file
    File.WriteAllText(filePath, jsonString);
    Console.WriteLine("Default guests saved to guests.json!");
}

// Display the current state of the list
Console.WriteLine("\n--- Current Guest List ---");
foreach (Guest guest in guestsList)
{
    guest.DisplayInfo();
}

// Let's add a new guest dynamically to test persistence on the next run
Console.WriteLine("\nAdd a new guest for the next execution:");
Console.Write("Enter name: ");
string? nameInput = Console.ReadLine();

Console.Write("Enter age: ");
string? ageInput = Console.ReadLine();

if (!string.IsNullOrWhiteSpace(nameInput) && int.TryParse(ageInput, out int age))
{
    // Add the new guest to our memory list
    guestsList.Add(new Guest(nameInput, age));
    
    // Save the updated list back to the file
    var options = new JsonSerializerOptions { WriteIndented = true };
    string updatedJson = JsonSerializer.Serialize(guestsList, options);
    File.WriteAllText(filePath, updatedJson);
    
    Console.WriteLine($"Successfully added {nameInput} and saved to disk!");
}
