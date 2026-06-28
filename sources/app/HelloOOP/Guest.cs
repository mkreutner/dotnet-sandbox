public class Guest
{
  public string FirstName { get; set; }
  public int Age { get; set; }

  public Guest(string firstName, int age)
  {
    FirstName = firstName;
    Age = age;
  }

  public void Introduce()
  {
    Console.WriteLine($"- {FirstName} ({Age} years)");
  }
}
