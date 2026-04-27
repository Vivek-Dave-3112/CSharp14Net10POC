namespace CSharp14Features.Demos;

// =====================================================================
// C# 14 - Null-Conditional Assignment
// ---------------------------------------------------------------------
// You can now use ?. and ?[] on the LEFT side of an assignment or
// compound assignment. The assignment happens ONLY if the left side
// is not null. If it is null, the right-hand side is NOT evaluated.
// =====================================================================

public class UserProfile
{
    public string? DisplayName { get; set; }
    public List<string> Tags { get; } = new();
    public int LoginCount { get; set; }
}

public static class NullConditionalAssignmentDemo
{
    public static void Run()
    {
        UserProfile? profile = new();

        // Traditional (pre C# 14):
        // if (profile != null) profile.DisplayName = "Vivek";

        // C# 14 null-conditional assignment:
        profile?.DisplayName = "Vivek";
        Console.WriteLine($"After assignment, DisplayName = {profile?.DisplayName}");

        // Works with compound assignment too
        profile?.LoginCount += 1;
        profile?.LoginCount += 1;
        Console.WriteLine($"LoginCount after two increments = {profile?.LoginCount}");

        // Null target - the assignment is SKIPPED silently,
        // and the right-hand expression is NOT evaluated.
        UserProfile? missing = null;
        missing?.DisplayName = GetValueAndLog("assigning to missing");
        Console.WriteLine("(above line printed nothing because target was null)");

        // ?[] with indexer-style assignment
        profile?.Tags.Add("c#14");
        Console.WriteLine($"Tags count = {profile?.Tags.Count}");
    }

    private static string GetValueAndLog(string context)
    {
        Console.WriteLine($"GetValueAndLog called: {context}");
        return "evaluated!";
    }
}
