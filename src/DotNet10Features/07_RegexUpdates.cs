using System.Text.RegularExpressions;

namespace DotNet10Features.Demos;

// =====================================================================
// .NET 10 - Regex improvements
// ---------------------------------------------------------------------
// Headline items:
//   - [GeneratedRegex] source generator (from .NET 7+, still recommended)
//     - compiles the regex into IL at build time
//     - no reflection / runtime codegen / AOT-friendly
//   - Performance improvements in the backtracking engine and NonBacktracking
//   - Span-based Regex.EnumerateMatches / EnumerateSplits avoid allocations
// =====================================================================

public static partial class RegexUpdatesDemo
{
    // Compile-time generated regex - faster than Regex("...") and AOT-safe.
    [GeneratedRegex(@"\b(\w+)@(\w+\.\w+)\b", RegexOptions.IgnoreCase)]
    private static partial Regex EmailRegex();

    public static void Run()
    {
        const string text = "Contact me at Vivek@EXAMPLE.com or support@data-robots.io for help.";

        Console.WriteLine("Using [GeneratedRegex] partial method:");
        foreach (Match m in EmailRegex().Matches(text))
        {
            Console.WriteLine($"  matched: {m.Value}  (user='{m.Groups[1]}', domain='{m.Groups[2]}')");
        }

        // --- Span-based, allocation-free enumeration ---
        Console.WriteLine("\nSpan-based EnumerateMatches:");
        foreach (var match in EmailRegex().EnumerateMatches(text))
        {
            var slice = text.AsSpan(match.Index, match.Length);
            Console.WriteLine($"  index={match.Index} length={match.Length} -> {slice}");
        }
    }
}
