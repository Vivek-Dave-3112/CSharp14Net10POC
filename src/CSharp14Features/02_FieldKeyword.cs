namespace CSharp14Features.Demos;

// =====================================================================
// C# 14 - 'field' contextual keyword in property accessors
// ---------------------------------------------------------------------
// Before: you had to manually declare a private backing field to run
//         logic inside get/set.
// Now:    the compiler generates the backing field for you, accessible
//         inside the accessor via the 'field' keyword.
// Benefit: less boilerplate, no "stutter" naming (_name / Name).
// =====================================================================

public class Person
{
    // BEFORE C# 14 you would write:
    //   private string _name = "";
    //   public string Name
    //   {
    //       get => _name;
    //       set => _name = value?.Trim() ?? throw new ArgumentNullException();
    //   }

    // C# 14 - backing field is implicit, use 'field'
    public string Name
    {
        get => field ?? "";
        set => field = value?.Trim()
            ?? throw new ArgumentNullException(nameof(value));
    }

    // Works for auto-properties with validation too
    public int Age
    {
        get;
        set => field = value < 0
            ? throw new ArgumentOutOfRangeException(nameof(value), "Age cannot be negative")
            : value;
    }

    // Lazy-init pattern: no backing field declared by you
    public string DisplayName
    {
        get => field ??= $"{Name} ({Age})";
    }
}

public static class FieldKeywordDemo
{
    public static void Run()
    {
        var p = new Person { Name = "  Vivek  ", Age = 30 };

        Console.WriteLine($"Name (trimmed by setter): '{p.Name}'");
        Console.WriteLine($"Age: {p.Age}");
        Console.WriteLine($"DisplayName (lazy): {p.DisplayName}");

        try
        {
            p.Age = -5;
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"Caught expected error: {ex.Message.Split('(')[0].Trim()}");
        }
    }
}
