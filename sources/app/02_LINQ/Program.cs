// File: Program.cs
using System.Text.Json;

string filePath = "guests.json";
List<Guest> guestsList;

Console.WriteLine("--- .NET 10 LINQ Lab ---");

// 1. Load data from JSON (from our previous step)
if (File.Exists(filePath))
{
    string jsonString = File.ReadAllText(filePath);
    guestsList = JsonSerializer.Deserialize<List<Guest>>(jsonString) ?? new List<Guest>();
}
else
{
    guestsList = new List<Guest>
    {
        new Guest("Alice", 28),
        new Guest("Bob", 34),
        new Guest("Charlie", 22),
        new Guest("David", 19)
    };
}

// 2. LINQ OPERATION: Filtering with Where()
// The lambda expression 'g => g.Age > 25' means: "for each guest 'g', keep it if g.Age > 25"
List<Guest> olderGuests = guestsList.Where(g => g.Age > 25).ToList();

Console.WriteLine("\n--- LINQ: Guests older than 25 ---");
foreach (Guest guest in olderGuests)
{
    guest.DisplayInfo();
}

// 3. LINQ OPERATION: Sorting with OrderBy()
List<Guest> sortedGuests = guestsList.OrderBy(g => g.Age).ToList();

Console.WriteLine("\n--- LINQ: Guests sorted by age (ascending) ---");
foreach (Guest guest in sortedGuests)
{
    guest.DisplayInfo();
}

// 4. LINQ OPERATION: Projecting with Select()
// We transform a List<Guest> into a List<string> containing only the names
List<string> guestNames = guestsList.Select(g => g.Name).ToList();

Console.WriteLine("\n--- LINQ: Projecting names only ---");
foreach (string name in guestNames)
{
    Console.WriteLine($"- {name}");
}

// 5. BONUS: Aggregation (Count, Min, Max, Average)
double averageAge = guestsList.Average(g => g.Age);
Console.WriteLine($"\n[Stats] Average age of guests: {averageAge:F1} years old.");
