Console.WriteLine("Hello! What is your name? ");

string? firstname = Console.ReadLine();

if (!string.IsNullOrWhiteSpace(firstname))
{
  Console.WriteLine($"Nice to meet you {firstname} and welcome to .Net 10!");
}
else
{
  Console.WriteLine($"Oh, you don't want to tell me your name? No problem, hello anyway!");
}

///
/// 💡 Deux notions clés à retenir ici :
///
/// * Le string? (avec le point d'interrogation) :
///
///   En C#, cela signifie que la variable peut être
///   "nullable" (c'est-à-dire qu'elle peut être vide
///   ou égale à null). C'est une sécurité du langage
///   pour t'éviter des bugs plus tard.
///
/// * L'interpolation de chaîne ($"...") : 
///   Le petit symbole $ devant les guillemets permet
///   d'intégrer des variables directement au milieu
///   du texte en les mettant entre accolades {}.
///   C'est beaucoup plus propre que de faire des
///   concaténations avec des +.
///

Console.WriteLine("What year were you born?");
string? input = Console.ReadLine();

// int.TryParse allows you to convert text to a number (int) securely
if (int.TryParse(input, out int birthYear))
{
  int currentYear = 2026; // Yes, it's already 2026!

  int age = currentYear - birthYear;

  Console.WriteLine($"You are (or will be) {age} years old.");

  // A little loop for fun
  Console.WriteLine("Launching the .NET protocol in...");
  for (int i = 3; i > 0; i--)
  {
    Console.WriteLine($"{i}...");
    Thread.Sleep(500); // Pauses for 500 milliseconds (0.5s)
  }
  Console.WriteLine("🚀 Ready for the next part!");
}
else
{
  Console.WriteLine("That's not a valid year, cheater!");
}

///
/// 🧠 Ce qu'il faut noter ici :
///
/// * int.TryParse(...) :
///   En C#, on ne peut pas mélanger les serviettes
///   (du texte string) et les torchons (des entiers int).
///   Cette méthode tente de convertir la saisie.
///   Si ça réussit, elle crée la variable anneeNaissance
///   à la volée (out int anneeNaissance) et renvoie true.
///
/// * Thread.Sleep(500) :
///   C'est une fonction native bien pratique pour rythmer 
///   l'affichage dans la console.
///

///
/// Utiliser des fonctions
///

Console.WriteLine("What year were you born?");
string? input2 = Console.ReadLine();

if (int.TryParse(input2, out int birthYear2))
{
  int currentYear = 2026;
  int age = currentYear - birthYear2;

  Console.WriteLine($"You are (or will be) {age} years old.");

  // We CALL our function here, passing it a parameter (3 seconds)
  StartCountdown(3);
}
else
{
  Console.WriteLine("This is not a valid year!");
}

// --- OUR FUNCTIONS ARE DEFINED HERE ---

// 'void' means the function performs an action but returns no result.
// 'int seconds' is the parameter the function expects to work.
void StartCountdown(int seconds)
{
  Console.WriteLine("Launching the .NET protocol in...");

  for (int i = seconds; i > 0; i--)
  {
    Console.WriteLine($"{i}...");
    Thread.Sleep(500);
  }

  Console.WriteLine("🚀 Ready for the next step!");
}

///
/// Manipuler des listes en C#
///

// 1. Initializing an empty list of strings
List<string> guestsList = new List<string>();

Console.WriteLine("--- Guest Manager ---");
Console.WriteLine("Enter the guests' first names (type 'end' to finish):");

while (true)
{
  Console.Write("> ");

  string? input3 = Console.ReadLine();

  // If the user types 'end' (without regard to case), we exit the loop
  if (string.Equals(input3, "end", StringComparison.OrdinalIgnoreCase))
  {
    break;
  }

  // We check that the input is not empty before adding it
  if (!string.IsNullOrWhiteSpace(input3))
  {
    guestsList.Add(input3); // Adding to the list
    Console.WriteLine($"[Added] {input3} is now part of the list.");
  }

}

// 2. Processing and displaying list data
Console.WriteLine("\n--- Final guest list ---");
Console.WriteLine($"Total number of guests: {guestsList.Count}");

// The 'foreach' loop allows you to iterate through the list element by element
foreach (string guest in guestsList)
{
  Console.WriteLine($"- {guest}");
}

