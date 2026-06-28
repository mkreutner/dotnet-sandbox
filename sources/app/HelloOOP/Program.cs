// This time, our list stores instances of our 'Invite' class
List<Guest> guestsFullList = new List<Guest>();

Console.WriteLine("--- VIP Guest Registration ---");

// Let's manually add two guests to test our class
Guest guest1 = new Guest("Alice", 28);
Guest guest2 = new Guest("Bob", 34);

guestsFullList.Add(guest1);
guestsFullList.Add(guest2);


// We can also add directly on the fly
guestsFullList.Add(new Guest("Charlie", 22));

Console.WriteLine("\n--- Displaying VIP Profiles ---");

foreach (Guest g in guestsFullList)
{
  // each object uses its own method to display itself
  g.Introduce();
}
