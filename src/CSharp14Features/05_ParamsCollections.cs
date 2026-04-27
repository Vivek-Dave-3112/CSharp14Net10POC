namespace CSharp14Features.Demos;

// =====================================================================
// C# 13 recap (still asked about in C# 14 interviews) - params collections
// ---------------------------------------------------------------------
// 'params' used to require an ARRAY (T[]). Since C# 13, it can be ANY
// collection type that the compiler can build via collection expressions:
//   IEnumerable<T>, IReadOnlyList<T>, List<T>, Span<T>, ReadOnlySpan<T>, ...
// This lets callers avoid an unnecessary heap allocation when a Span works.
// =====================================================================

public static class ParamsCollectionsHelpers
{
    // Old school - T[] allocation on every call
    public static int SumArray(params int[] values)
        => values.Sum();

    // C# 13 - ReadOnlySpan<int> avoids allocating an array for short calls
    public static int SumSpan(params ReadOnlySpan<int> values)
    {
        int total = 0;
        foreach (var v in values) total += v;
        return total;
    }

    // Any IEnumerable works too
    public static string JoinWords(params IEnumerable<string> words)
        => string.Join(", ", words);
}

public static class ParamsCollectionsDemo
{
    public static void Run()
    {
        Console.WriteLine($"SumArray(1,2,3) = {ParamsCollectionsHelpers.SumArray(1, 2, 3)}");

        // This call does NOT allocate a heap array - compiler uses a stack span.
        Console.WriteLine($"SumSpan(1,2,3,4,5) = {ParamsCollectionsHelpers.SumSpan(1, 2, 3, 4, 5)}");

        Console.WriteLine($"JoinWords(\"a\",\"b\",\"c\") = " +
            ParamsCollectionsHelpers.JoinWords("a", "b", "c"));
    }
}
