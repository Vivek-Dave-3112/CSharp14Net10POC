namespace CSharp14Features.Demos;

// =====================================================================
// C# 14 - Unbound generic types in nameof(...)
// ---------------------------------------------------------------------
// You can now write nameof(List<>) or nameof(Dictionary<,>) without
// providing type arguments. Before, you had to use nameof(List<int>)
// which felt awkward when you only wanted the type name.
// =====================================================================

public static class UnboundGenericNameofDemo
{
    public static void Run()
    {
        // C# 14 - no dummy type arguments needed
        Console.WriteLine($"nameof(List<>)         = {nameof(List<>)}");
        Console.WriteLine($"nameof(Dictionary<,>)  = {nameof(Dictionary<,>)}");
        Console.WriteLine($"nameof(IEnumerable<>)  = {nameof(IEnumerable<>)}");

        // Before C# 14 you had to invent dummy generics, which was ugly:
        //   nameof(List<int>)  // returned "List", but required <int>
        Console.WriteLine($"nameof(List<int>) still works too = {nameof(List<int>)}");
    }
}
